using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRabbitMQ.Domain.Entities.NotasFiscais.Business
{
    public interface INotaFiscalBusiness
    {
        Task AddNotaFiscal(NotaFiscal nota);
    }
}
