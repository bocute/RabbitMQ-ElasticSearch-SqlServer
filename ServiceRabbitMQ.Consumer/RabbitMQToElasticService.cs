using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceRabbitMQ.Configurations;
using ServiceRabbitMQ.Domain.Entities;
using ServiceRabbitMQ.Domain.Entities.NotasFiscais.Business;
using ServiceRabbitMQ.DTO;

namespace ServiceRabbitMQ
{
    public class RabbitMQToElasticService : BackgroundService
    {
        private readonly ILogger<RabbitMQToElasticService> _logger;
        private readonly RabbitMqOptions _rabbitOptions;
        private readonly IMapper _mapper;
        private readonly INotaFiscalBusiness _notaFiscalBusiness;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQToElasticService(
            ILogger<RabbitMQToElasticService> logger, 
            RabbitMqOptions rabbitOptions,
            IMapper mapper,
            INotaFiscalBusiness notaFiscalBusiness)
        {
            _logger = logger;
            _rabbitOptions = rabbitOptions;
            _mapper = mapper;
            _notaFiscalBusiness = notaFiscalBusiness;
            InitRabbitMQ(); 
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message  
                var content = System.Text.Encoding.UTF8.GetString(ea.Body);

                // handle the received message  
                HandleMessage(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_rabbitOptions.QueueDeclare, false, consumer);
            return Task.CompletedTask;
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

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_rabbitOptions.ExchangeDeclare, ExchangeType.Topic);
            _channel.QueueDeclare(_rabbitOptions.QueueDeclare, false, false, false, null);
            _channel.QueueBind(_rabbitOptions.QueueDeclare, _rabbitOptions.ExchangeDeclare, _rabbitOptions.RountingKey, null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        private void HandleMessage(string content)
        {  
            _logger.LogInformation($"Integrando nota fiscal {content}");
            try
            {
                var notaFiscal = JsonSerializer.Deserialize<NotaFiscalDto>(content);
                _notaFiscalBusiness.AddNotaFiscal(_mapper.Map<NotaFiscal>(notaFiscal));

                _logger.LogInformation($"Nota Fiscal - Número: {notaFiscal.Numero}, " +
                    $"Série: {notaFiscal.Serie}, " +
                    $"Série ECF: {notaFiscal.SerieEcf}, " +
                    $"Empresa: {notaFiscal.NroEmpresa} integrada com sucesso.");

            }
            catch (Exception)
            {
                _logger.LogInformation($"Erro ao integrar nota fiscal {content}");
            }
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
