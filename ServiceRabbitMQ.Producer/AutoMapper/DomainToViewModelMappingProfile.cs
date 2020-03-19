using AutoMapper;
using ServiceRabbitMQ.Domain.Entities;
using ServiceRabbitMQ.Producer.DTO;

namespace ServiceRabbitMQ.AutoMapper
{
    internal class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<NotaFiscal, NotaFiscalDto>();
        }
    }
}