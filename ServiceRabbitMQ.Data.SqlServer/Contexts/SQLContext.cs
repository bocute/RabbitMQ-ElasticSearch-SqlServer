
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceRabbitMQ.Data.SqlServer.Mappings;
using ServiceRabbitMQ.Domain.Entities;
using System.IO;

namespace ServiceRabbitMQ.Data.SqlServer.Contexts
{
    public class SQLContext : DbContext
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NotasFiscaisMap());
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}
