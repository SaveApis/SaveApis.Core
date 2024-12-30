using Autofac;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Application.Hangfire.Events;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Extensions;
using SaveApis.Core.Infrastructure.Persistence.Sql.Factories;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;
using SaveApis.Core.Persistence.Sql.Manager;

namespace SaveApis.Core.Application.DI;

public class EfCoreModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(WebApplicationBuilderExtensions.CustomAssemblies.ToArray())
            .Where(type => type.GetInterfaces().Any(it =>
                it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IDbFactory<>)))
            .As<IDbFactory<DbContext>>()
            .AsImplementedInterfaces();

        builder.RegisterType<DbManager>().As<IDbManager>();

        builder.RegisterBuildCallback(async scope =>
        {
            foreach (var factory in scope.Resolve<IEnumerable<IDbFactory<DbContext>>>())
            {
                var context = factory.CreateDbContext([]);
                await context.Database.MigrateAsync().ConfigureAwait(false);
            }

            var mediator = scope.Resolve<IMediator>();
            await mediator.Publish(new MigrationCompletedEvent()).ConfigureAwait(false);
        });
    }
}
