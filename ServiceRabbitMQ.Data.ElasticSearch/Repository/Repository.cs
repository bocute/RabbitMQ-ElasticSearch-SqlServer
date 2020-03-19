using Microsoft.Extensions.Hosting;
using Nest;
using ServiceRabbitMQ.Domain.Entities;
using ServiceRabbitMQ.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRabbitMQ.Data.ElasticSeaarch.Repository
{
    public abstract class Repository<T> : Domain.Interfaces.IRepository<T> where T : Entity
    {
        private readonly IElasticClient _elasticClient;
        private readonly IHostEnvironment _env;

        public Repository(IElasticClient elasticClient, IHostEnvironment env)
        {
            _elasticClient = elasticClient;
            _env = env;
        }
        public async Task Add(T obj)
        {
            string filePath = GetFilePath(obj);

            bool postExists = File.Exists(filePath);

            if (postExists)
                await _elasticClient.UpdateAsync<T>(obj, u => u.Doc(obj));
            else
                await _elasticClient.IndexDocumentAsync(obj);
        }

        public void Dispose()
        {
            
        }

        public Task<IEnumerable<T>> GetAllAsync(int? pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        private string GetFilePath(T obj)
        {
            return Path.Combine(Path.Combine(_env.ContentRootPath, obj.WebRootPath), obj.Id + ".xml");
        }
    }
}
