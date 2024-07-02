using AutoMapper;
using SaveApis.Core.Example.Application.Models;
using SaveApis.Core.Example.Application.Models.DTO;

namespace SaveApis.Core.Example.Application.Mapper;

public class ExampleMapperProfile : Profile
{
    public ExampleMapperProfile() : base("Example Mapper Profile")
    {
        CreateMap<ExampleMapperModel, ExampleMapperDto>();
    }
}