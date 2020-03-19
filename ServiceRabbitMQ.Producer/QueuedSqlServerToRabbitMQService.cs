using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceRabbitMQ.Configurations;
using ServiceRabbitMQ.Domain.Entities.NotasFiscais.Repository;
using ServiceRabbitMQ.Producer.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRabbitMQ.Producer
{
    public class QueuedSqlServerToRabbitMQService : IHostedService, IDisposable
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<QueuedSqlServerToRabbitMQService> _logger;
        private readonly IMapper _mapper;
        private readonly RabbitMqOptions _rabbitOptions;
        private IConnection _connection;
        private IModel _channel;
        private Timer _timer;
        private int executionCount = 0;
        private CancellationToken _cancellationToken;

        public IServiceProvider Services { get; }

        public QueuedSqlServerToRabbitMQService(IBackgroundTaskQueue taskQueue,
        ILogger<QueuedSqlServerToRabbitMQService> logger,
        IMapper mapper,
        RabbitMqOptions rabbitOptions,
        IServiceProvider services)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _mapper = mapper;
            _rabbitOptions = rabbitOptions;
            Services = services;

            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitOptions.HostName,
                UserName = _rabbitOptions.User,
                Password = _rabbitOptions.Password,
                Port = _rabbitOptions.Port,
                VirtualHost = _rabbitOptions.VirtualHost,
                ContinuationTimeout = new TimeSpan(10, 0, 0, 0)
            };

            _connection = factory.CreateConnection();
 
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_rabbitOptions.ExchangeDeclare, ExchangeType.Topic);
            _channel.QueueDeclare(_rabbitOptions.QueueDeclare, false, false, false, null);
            _channel.QueueBind(_rabbitOptions.QueueDeclare, _rabbitOptions.ExchangeDeclare, _rabbitOptions.RountingKey, null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando exportação de dados");

            _cancellationToken = cancellationToken;

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            const int pageSize = 3;

            try
            {
                var page = 1;
                while (page != 0)
                {
                    // Implementado scope para evitar problema de fazer uma chamada antes de outra terminar(DBContext singleton), possibilitando a injeção de dependecia scoped.
                    using (var scope = Services.CreateScope())
                    {
                        var scopedRepository = scope.ServiceProvider.GetRequiredService<INotaFiscalRepository>();
                        ICollection<NotaFiscalDto> notasFiscais = _mapper.Map<ICollection<NotaFiscalDto>>(await scopedRepository.GetAllAsync(page, pageSize));

                        if (notasFiscais.Count > 0)
                        {
                            page = notasFiscais.Count < pageSize ? 0 : page + 1;

                            _taskQueue.QueueBackgroundWorkItem(async token =>
                            {
                                await Task.Run(() =>
                                {
                                    foreach (var item in notasFiscais)
                                    {
                                        if (!token.IsCancellationRequested)
                                        {
                                            _logger.LogInformation($"Exportação da nota {item.Numero.ToString()} iniciada.");
                                            var message = JsonSerializer.Serialize(item);

                                            var body = Encoding.UTF8.GetBytes(message);

                                            _channel.BasicPublish(exchange: _rabbitOptions.ExchangeDeclare,
                                                                  routingKey: _rabbitOptions.RountingKey,
                                                                  basicProperties: null,
                                                                  body: body);

                                            _logger.LogInformation($"Nota {item.Numero.ToString()} adicionada à fila.");
                                        }
                                    }
                                }, _cancellationToken);
                            });
                        }

                    }
                }

                await BackgroundProcessing(_cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Erro ao exportar notas: " + ex);

                await StopAsync(_cancellationToken);
            }
        }

        private async Task BackgroundProcessing(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem =
                    await _taskQueue.DequeueAsync(cancellationToken);

                try
                {
                    await workItem(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Ocorreu um erro durante a execução {WorkItem}.", nameof(workItem));
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Parando exportação de dados.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;

        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public void Dispose()
        {
            _logger.LogInformation("Parando exportação de dados.");
            _timer?.Dispose();
        }


    }
}
