using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRabbitMQ.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public string WebRootPath { get; protected set; }
    }
}
