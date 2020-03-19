using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRabbitMQ.Configurations
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string ExchangeDeclare { get; set; }
        public string QueueDeclare { get; set; }
        public string RountingKey { get; set; }
    }
}
