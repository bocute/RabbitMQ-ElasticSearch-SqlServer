using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceRabbitMQ.Configurations;
using ServiceRabbitMQ.Producer.Configurations;

namespace ServiceRabbitMQ.Producer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    RabbitMqOptions rabbitOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();

                    services.AddSingleton(rabbitOptions);
                    services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
                    services.AddHostedService<QueuedSqlServerToRabbitMQService>();
                    services.AddAutoMapper(typeof(Program));

                    services.AddDIConfiguration();
                });
    }
}
