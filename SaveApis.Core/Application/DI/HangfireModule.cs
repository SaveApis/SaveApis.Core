using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Correlate;
using Hangfire.Dashboard;
using Hangfire.Pro.Redis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using SaveApis.Core.Application.Hangfire;
using SaveApis.Core.Application.Hangfire.Events;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Extensions;
using SaveApis.Core.Infrastructure.Hangfire.Jobs;
using Serilog;

namespace SaveApis.Core.Application.DI;

public class HangfireModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register Hangfire services
        var collection = CreateHangfireCollection();
        builder.Populate(collection);

        // Register authorization filters
        // There are no authorization filter in Core so we only register the filters from the custom assemblies
        builder.RegisterAssemblyTypes(WebApplicationBuilderExtensions.CustomAssemblies.ToArray())
            .Where(type => type.IsAssignableTo<IDashboardAuthorizationFilter>() || type.IsAssignableTo<IDashboardAsyncAuthorizationFilter>())
            .AsImplementedInterfaces()
            .InstancePerDependency();

        // Register jobs
        builder.RegisterAssemblyTypes(WebApplicationBuilderExtensions.AllAssemblies.ToArray())
            .Where(type => type.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IJob<>)))
            .AsImplementedInterfaces()
            .InstancePerDependency();

        builder.RegisterBuildCallback(scope => GlobalConfiguration.Configuration.UseAutofacActivator(scope));
        builder.RegisterBuildCallback(async scope =>
        {
            var mediator = scope.Resolve<IMediator>();
            await mediator.Publish(new ApplicationStartedEvent()).ConfigureAwait(false);
        });
    }

    private ServiceCollection CreateHangfireCollection()
    {
        var hangfireName = Configuration["hangfire_name"] ?? "SaveApis";
        var hangfireRedisHost = Configuration["hangfire_redis_host"] ?? "localhost";
        var hangfireRedisPort = Configuration["hangfire_redis_port"] ?? "6379";
        var hangfireRedisDatabase = int.TryParse(Configuration["hangfire_redis_database"], out var redisDatabase) ? redisDatabase : 0;
        var hangfireRedisPrefix = Configuration["hangfire_redis_prefix"] ?? "saveapis:hangfire:";
        hangfireRedisPrefix = hangfireRedisPrefix.EndsWith(':')
            ? hangfireRedisPrefix
            : $"{hangfireRedisPrefix}:";

        var collection = new ServiceCollection();

        collection.AddHangfire((provider, globalConfiguration) =>
        {
            globalConfiguration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            globalConfiguration.UseSimpleAssemblyNameTypeSerializer();
            globalConfiguration.UseRecommendedSerializerSettings(settings => settings.Converters.Add(new StringEnumConverter()));
            globalConfiguration.UseRedisStorage($"{hangfireRedisHost}:{hangfireRedisPort}", new RedisStorageOptions
            {
                Database = hangfireRedisDatabase,
                Prefix = hangfireRedisPrefix,
            }).WithJobExpirationTimeout(TimeSpan.FromDays(7));
            globalConfiguration.UseCorrelate(provider);
        });
        collection.AddHangfireServer((_, options) =>
        {
            options.WorkerCount = Environment.ProcessorCount * 20;
            options.Queues = Enum.GetValues<HangfireQueue>()
                .OrderByDescending(queue => (int)queue)
                .Select(it => it.ToString().ToLowerInvariant())
                .ToArray();
            options.ServerName = hangfireName;
        });

        return collection;
    }

    public override void PostAction(WebApplication application)
    {
        var path = Configuration["hangfire_path"] ?? "/hangfire";
        path = path.StartsWith('/') ? path : $"/{path}";

        var scope = application.Services.CreateScope();
        var authorizationFilters = application.Environment.IsDevelopment()
            ? []
            : scope.ServiceProvider.GetRequiredService<IEnumerable<IDashboardAuthorizationFilter>>().ToList();
        var asyncAuthorizationFilters = application.Environment.IsDevelopment()
            ? []
            : scope.ServiceProvider.GetRequiredService<IEnumerable<IDashboardAsyncAuthorizationFilter>>().ToList();

        if (!application.Environment.IsDevelopment() && authorizationFilters.Count == 0 && asyncAuthorizationFilters.Count == 0)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
            logger.Warning("Hangfire dashboard is not protected by any authorization filter!");
        }

        var options = new DashboardOptions
        {
            Authorization = [.. authorizationFilters],
            AsyncAuthorization = [.. asyncAuthorizationFilters],
        };

        application.UseHangfireDashboard(path, options);
    }
}
