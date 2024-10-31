using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Infrastructure.DI;

namespace SaveApis.Core.Application.DI;

public class FluentValidatorModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddValidatorsFromAssemblies(WebApplicationBuilderExtension.Assemblies);

        builder.Populate(collection);
    }
}