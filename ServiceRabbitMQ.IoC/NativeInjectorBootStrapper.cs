using ServiceRabbitMQ.Data.SqlServer.Contexts;
using ServiceRabbitMQ.Domain.Entities.NotasFiscais.Business;
using ServiceRabbitMQ.Domain.Entities.NotasFiscais.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Elastic
            services.AddSingleton<INotaFiscalRepository, ServiceRabbitMQ.Data.ElasticSeaarch.Repository.NotasFiscais.NotaFiscalRepository>();
            services.AddSingleton<INotaFiscalBusiness, NotaFiscalBusiness>();
        }

        public static void RegisterServicesSqlServer(IServiceCollection services)
        {
            //Sql Server
            services.AddScoped<INotaFiscalRepository, ServiceRabbitMQ.Data.SqlServer.Repository.NotasFiscais.NotaFiscalRepository>();
            services.AddScoped<SQLContext>();
        }
    }
}
