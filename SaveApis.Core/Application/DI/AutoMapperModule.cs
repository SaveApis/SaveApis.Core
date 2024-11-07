using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Infrastructure.DI;

namespace SaveApis.Core.Application.DI;

public class AutoMapperModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        builder.RegisterAutoMapper(false, ContainerBuilderExtension.Assemblies);
    }
}