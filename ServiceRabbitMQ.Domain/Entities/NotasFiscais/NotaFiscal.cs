using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRabbitMQ.Domain.Entities
{
    public class NotaFiscal : Entity
    {
        public long Numero { get; private set; }
        public string Serie { get; private set; }
        public string SerieEcf { get; private set; }
        public long NroEmpresa { get; private set; }

        public static class NotaFiscalFactory
        {
            public static NotaFiscal AddNotaFiscal(long numero, string serie, string serieEcf, long nroEmpresa)
            {
                var nota = new NotaFiscal()
                {
                    Id = Guid.NewGuid(),
                    Numero = numero,
                    Serie = serie,
                    SerieEcf = serieEcf,
                    NroEmpresa = nroEmpresa,
                    WebRootPath = Enum.GetName(typeof(WebRootPathEnum), WebRootPathEnum.Notas)
            };

                return nota;
            }
        }
    }
}
