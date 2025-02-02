using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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
            var logger = scope.Resolve<ILogger>();

            foreach (var factory in scope.Resolve<IEnumerable<IDesignTimeDbContextFactory<BaseDbContext>>>().ToList())
            {
                using var context = factory.Create();

                logger.Information("{Context}: Migrate", context.GetType().Name);
                context.Database.Migrate();
                logger.Information("{Context}: Migrate - Done", context.GetType().Name);
            }
        });
    }
}
