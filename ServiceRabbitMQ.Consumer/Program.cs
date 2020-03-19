using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceRabbitMQ.Configurations;

namespace ServiceRabbitMQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    RabbitMqOptions rabbitOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();

                    services.AddHostedService<RabbitMQToElasticService>();

                    services.AddSingleton(rabbitOptions);
                    services.AddElasticsearch(configuration);
                    services.AddAutoMapper(typeof(Program));

                    services.AddDIConfiguration();
                });
    }
}
