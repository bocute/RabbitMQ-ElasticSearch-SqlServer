using ServiceRabbitMQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRabbitMQ.Domain.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task Add(T obj);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(int? pageNumber, int pageSize);
    }
}
