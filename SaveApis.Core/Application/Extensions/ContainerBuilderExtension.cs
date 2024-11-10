using System.Collections.ObjectModel;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Application.DI;

namespace SaveApis.Core.Application.Extensions;

public static class ContainerBuilderExtension
{
    private static readonly Collection<Assembly> AssemblyStorage = [Assembly.GetExecutingAssembly()];
    internal static Assembly[] Assemblies => AssemblyStorage.Distinct().ToArray();

    public static ContainerBuilder WithAssemblies(this ContainerBuilder builder, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies) AssemblyStorage.Add(assembly);

        return builder;
    }

    public static ContainerBuilder WithAutoMapper(this ContainerBuilder builder, IConfiguration configuration)
    {
        builder.RegisterModule(new AutoMapperModule(configuration));
        return builder;
    }

    public static ContainerBuilder WithFluentValidator(this ContainerBuilder builder, IConfiguration configuration)
    {
        builder.RegisterModule(new FluentValidatorModule(configuration));
        return builder;
    }

    public static ContainerBuilder WithSignalR(this ContainerBuilder builder, IConfiguration configuration)
    {
        builder.RegisterModule(new SignalRModule(configuration));
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

    public static ContainerBuilder WithGraphQl<TQuery, TMutation>(this ContainerBuilder builder,
        IConfiguration configuration) where TMutation : class where TQuery : class
    {
        builder.RegisterModule(new GraphQlModule<TQuery, TMutation>(configuration));
        return builder;
    }

    public static ContainerBuilder WithSwagger(this ContainerBuilder builder, IConfiguration configuration)
    {
        builder.RegisterModule(new SwaggerModule(configuration));
        return builder;
    }
}