using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRabbitMQ.Producer.DTO
{
    public class NotaFiscalDto
    {
        public long Numero { get; set; }
        public string Serie { get; set; }
        public string SerieEcf { get; set; }
        public long NroEmpresa { get; set; }
    }
}
