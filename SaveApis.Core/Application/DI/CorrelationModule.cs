using Autofac;
using Autofac.Extensions.DependencyInjection;
using Correlate.AspNetCore;
using Correlate.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Infrastructure.DI;

namespace SaveApis.Core.Application.DI;

public class CorrelationModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();

        services.AddCorrelate();
        services.ConfigureHttpClientDefaults(clientBuilder => clientBuilder.CorrelateRequests());

        services.AddMvcCore(options => options.EnableEndpointRouting = false);

        builder.Populate(services);
    }

    public override void PostAction(WebApplication application)
    {
        application.UseCorrelate();

        application.UseMvc();
    }
}
