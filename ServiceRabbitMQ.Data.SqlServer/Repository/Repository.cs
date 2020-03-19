using Microsoft.EntityFrameworkCore;
using ServiceRabbitMQ.Data.SqlServer.Contexts;
using ServiceRabbitMQ.Domain.Entities;
using ServiceRabbitMQ.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRabbitMQ.Data.SqlServer.Repository
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected SQLContext Db;
        protected DbSet<T> DbSet;

        public Repository(SQLContext context)
        {
            Db = context;
            DbSet = Db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(int? pageNumber, int pageSize)
        {
            return await PaginatedList<T>.CreateAsync(DbSet.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(DbSet.Find(id));
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public Task Add(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
