using Autofac;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Application.DI;

namespace SaveApis.Core.Application.Extensions;

public static class ContainerBuilderExtension
{
    public static ContainerBuilder WithAutoMapper(this ContainerBuilder builder)
    {
        builder.RegisterModule<AutoMapperModule>();
        return builder;
    }

    public static ContainerBuilder WithFluentValidator(this ContainerBuilder builder)
    {
        builder.RegisterModule<FluentValidatorModule>();
        return builder;
    }

    public static ContainerBuilder WithSignalR(this ContainerBuilder builder)
    {
        builder.RegisterModule<SignalRModule>();
        return builder;
    }

    public static ContainerBuilder WithEfCore(this ContainerBuilder builder, IConfiguration configuration)
    {
        builder.RegisterModule(new EfCoreModule(configuration));
        return builder;
    }
    public static ContainerBuilder WithMongo(this ContainerBuilder builder, IConfiguration configuration)
    {
        builder.RegisterModule(new MongoModule(configuration));
        return builder;
    }
}