using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using SaveApis.Core.Application.Extensions;

namespace SaveApis.Core.Application.DI;

public class AutoMapperModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAutoMapper(false, WebApplicationBuilderExtension.Assemblies.ToArray());
    }
}