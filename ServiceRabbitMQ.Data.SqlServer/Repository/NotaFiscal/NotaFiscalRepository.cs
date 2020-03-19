using ServiceRabbitMQ.Data.SqlServer.Contexts;
using ServiceRabbitMQ.Domain.Entities;
using ServiceRabbitMQ.Domain.Entities.NotasFiscais.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRabbitMQ.Data.SqlServer.Repository.NotasFiscais
{
    public class NotaFiscalRepository : Repository<NotaFiscal>, INotaFiscalRepository
    {
        public NotaFiscalRepository(SQLContext context) : base(context)
        {
        }
    }
}
