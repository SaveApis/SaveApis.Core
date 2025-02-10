using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Console;
using Hangfire.Correlate;
using Hangfire.Dashboard;
using Hangfire.Pro.Redis;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using SaveApis.Core.Common.Application.Hangfire.Events;
using SaveApis.Core.Common.Application.Hangfire.Types;
using SaveApis.Core.Common.Application.Types;
using SaveApis.Core.Common.Infrastructure.DI;
using SaveApis.Core.Common.Infrastructure.Hangfire.Jobs;

namespace SaveApis.Core.Common.Application.DI;

public class HangfireModule(IConfiguration configuration, ServerType serverType, IEnumerable<Assembly> assemblies) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterHangfire(builder);

        switch (serverType)
        {
            case ServerType.Worker:
                RegisterHangfireWorker(builder);

                break;
            case ServerType.Backend:
            {
                var assemblyList = assemblies.ToArray();

                builder
                    .RegisterAssemblyTypes(assemblyList)
                    .Where(type => type.IsAssignableTo<IDashboardAuthorizationFilter>() || type.IsAssignableTo<IDashboardAsyncAuthorizationFilter>())
                    .AsImplementedInterfaces();

                break;
            }
        }

        builder.RegisterAssemblyTypes(assemblies.ToArray())
            .Where(type => type.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IJob<>)))
            .AsImplementedInterfaces();

        builder.RegisterBuildCallback(scope => GlobalConfiguration.Configuration.UseAutofacActivator(scope));
        builder.RegisterBuildCallback(scope =>
        {
            var mediator = scope.Resolve<IMediator>();

            mediator.Publish(new ApplicationStartedEvent(serverType)).GetAwaiter().GetResult();
        });
    }

    private void RegisterHangfire(ContainerBuilder builder)
    {
        var server = configuration["hangfire_redis_server"] ?? string.Empty;
        var port = configuration["hangfire_redis_port"] ?? string.Empty;
        var database = configuration["hangfire_redis_database"] ?? string.Empty;
        var prefix = configuration["hangfire_redis_prefix"] ?? string.Empty;

        var collection = new ServiceCollection();

        var options = new RedisStorageOptions
        {
            Database = int.TryParse(database, out var d) ? d : 0,
            Prefix = prefix.EndsWith(':') ? prefix : $"{prefix}:",
        };

        collection.AddHangfire((serviceProvider, globalConfiguration) =>
        {
            globalConfiguration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings(settings => settings.Converters.Add(new StringEnumConverter()))
                .UseRedisStorage($"{server}:{port}", options)
                .WithJobExpirationTimeout(TimeSpan.FromDays(7))
                .UseCorrelate(serviceProvider)
                .UseConsole(new ConsoleOptions
                {
                    BackgroundColor = "#23272e",
                    TextColor = "#FFFFFF",
                    TimestampColor = "#00FF00",
                })
                .UseThrottling();
        });

        builder.Populate(collection);
    }

    private static void RegisterHangfireWorker(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddHangfireServer(options =>
        {
            options.Queues = Enum.GetNames<HangfireQueue>().Select(it => it.ToLowerInvariant()).ToArray();
            options.WorkerCount = Environment.ProcessorCount * 20;
        });

        builder.Populate(collection);
    }
}
