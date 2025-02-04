using System.Reflection;
using Autofac;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SaveApis.Core.Common.Application.Hangfire.Events;
using SaveApis.Core.Common.Infrastructure.DI;
using SaveApis.Core.Common.Infrastructure.Extension;
using SaveApis.Core.Common.Infrastructure.Persistence.Sql;
using Serilog;

namespace SaveApis.Core.Common.Application.DI;

public class EfCoreModule(IEnumerable<Assembly> assemblies) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(assemblies.ToArray())
            .Where(type => type.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IDesignTimeDbContextFactory<>)))
            .AsImplementedInterfaces()
            .As<IDesignTimeDbContextFactory<BaseDbContext>>();

        builder.RegisterBuildCallback(scope =>
        {
            var mediator = scope.Resolve<IMediator>();
            var logger = scope.Resolve<ILogger>();

            var factories = scope.Resolve<IEnumerable<IDesignTimeDbContextFactory<BaseDbContext>>>().ToList();
            if (factories.Count == 0)
            {
                return;
            }

            foreach (var factory in factories)
            {
                using var context = factory.Create();

                logger.Information("{Context}: Migrate", context.GetType().Name);
                context.Database.Migrate();
                logger.Information("{Context}: Migrate - Done", context.GetType().Name);
            }

            mediator.Publish(new MigrationCompletedEvent()).GetAwaiter().GetResult();
        });
    }
}
