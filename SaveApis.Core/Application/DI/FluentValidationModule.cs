using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Extensions;

namespace SaveApis.Core.Application.DI;

public class FluentValidationModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddValidatorsFromAssemblies(WebApplicationBuilderExtensions.AllAssemblies);

        builder.Populate(collection);
    }
}
