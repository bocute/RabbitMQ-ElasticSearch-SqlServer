using ServiceRabbitMQ.Domain.Entities.NotasFiscais.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRabbitMQ.Domain.Entities.NotasFiscais.Business
{
    public class NotaFiscalBusiness : INotaFiscalBusiness
    {
        private readonly INotaFiscalRepository _notaFiscalRepository;

        public NotaFiscalBusiness(INotaFiscalRepository notaFiscalRepository)
        {
            _notaFiscalRepository = notaFiscalRepository;
        }
        public async Task AddNotaFiscal(NotaFiscal nota)
        {
            await _notaFiscalRepository.Add(NotaFiscal.NotaFiscalFactory.AddNotaFiscal(nota.Numero, nota.Serie, nota.SerieEcf, nota.NroEmpresa));
        }
    }
}
