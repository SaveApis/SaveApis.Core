using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Application.Validation;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Validation;

namespace SaveApis.Core.Application.DI;

public class FluentValidatorModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddValidatorsFromAssemblies(ContainerBuilderExtension.Assemblies);

        builder.Populate(collection);

        builder.RegisterType<ValidationFactory>().As<IValidationFactory>();
    }
}