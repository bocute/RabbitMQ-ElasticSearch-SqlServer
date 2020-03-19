using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using ServiceRabbitMQ.Domain.Entities;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NotasElasticSearchExtension
    {
        public static void AddElasticsearch(
                this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<NotaFiscal>(m => m
                    .Ignore(p => p.WebRootPath)
                    .PropertyName(p => p.Id, "id")
                    .PropertyName(p => p.Numero, "numero")
                    .PropertyName(p => p.Serie, "serie")
                    .PropertyName(p => p.SerieEcf, "serieecf")
                    .PropertyName(p => p.NroEmpresa, "nroempresa")
                );

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
