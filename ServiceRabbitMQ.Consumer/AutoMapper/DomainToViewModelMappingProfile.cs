using AutoMapper;
using ServiceRabbitMQ.Domain.Entities;
using ServiceRabbitMQ.DTO;

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