using AutoMapper;
using SaveApis.Core.Example.Domains.Models;
using SaveApis.Core.Example.Domains.Models.DTO;

namespace SaveApis.Core.Example.Application.Mapper;

public class ExampleMapperProfile : Profile
{
    public ExampleMapperProfile() : base("Example Mapper Profile")
    {
        CreateMap<ExampleMapperModel, ExampleMapperDto>();
    }
}