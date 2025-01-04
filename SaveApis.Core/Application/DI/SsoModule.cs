using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Application.Sso;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Sso;

namespace SaveApis.Core.Application.DI;

public class SsoModule(IConfiguration configuration) : BaseModule(configuration)
{
    private const string Url = "https://sso.saveapis.de/graphql/";

    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        var services = new ServiceCollection();
        services.AddSsoClient()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(Url));

        builder.Populate(services);

        builder.RegisterType<SsoVerifier>().As<ISsoVerifier>();
    }
}
