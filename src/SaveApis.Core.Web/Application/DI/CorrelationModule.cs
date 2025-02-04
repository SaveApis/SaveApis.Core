using Autofac;
using Autofac.Extensions.DependencyInjection;
using Correlate.AspNetCore;
using Correlate.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Web.Infrastructure.DI;

namespace SaveApis.Core.Web.Application.DI;

public class CorrelationModule : BaseWebModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddCorrelate(options =>
        {
            options.IncludeInResponse = true;
        });

        collection.ConfigureHttpClientDefaults(clientBuilder => clientBuilder.CorrelateRequests());

        builder.Populate(collection);
    }

    protected override void PreAuthentication(WebApplication application)
    {
        application.UseCorrelate();
    }
}
