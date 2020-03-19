using AutoMapper;
using ServiceRabbitMQ.Domain.Entities;
using ServiceRabbitMQ.Producer.DTO;

namespace ServiceRabbitMQ.AutoMapper
{
    internal class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<NotaFiscalDto, NotaFiscal>();
        }
    }
}