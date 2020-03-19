using AutoMapper;
using ServiceRabbitMQ.Domain.Entities;
using ServiceRabbitMQ.DTO;

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