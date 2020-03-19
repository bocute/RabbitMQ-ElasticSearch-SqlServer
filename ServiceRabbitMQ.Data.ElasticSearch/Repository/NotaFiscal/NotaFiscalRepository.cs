
using Microsoft.Extensions.Hosting;
using Nest;
using ServiceRabbitMQ.Domain.Entities;
using ServiceRabbitMQ.Domain.Entities.NotasFiscais.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRabbitMQ.Data.ElasticSeaarch.Repository.NotasFiscais
{
    public class NotaFiscalRepository : Repository<NotaFiscal>, INotaFiscalRepository
    {
        public NotaFiscalRepository(IElasticClient elasticClient, IHostEnvironment env) 
            : base(elasticClient, env)
        {
        }
    }
}
