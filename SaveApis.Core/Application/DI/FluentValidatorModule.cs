using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Application.Extensions;

namespace SaveApis.Core.Application.DI;

public class FluentValidatorModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddValidatorsFromAssemblies(WebApplicationBuilderExtension.Assemblies);

        builder.Populate(collection);
    }
}