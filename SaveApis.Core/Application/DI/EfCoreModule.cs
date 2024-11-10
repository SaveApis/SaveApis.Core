using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Application.Jobs.MySql;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Persistence.MySql.Interfaces;
using SaveApis.Core.Persistence.MySql;

namespace SaveApis.Core.Application.DI;

public class EfCoreModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        builder.RegisterType<DbContextFactory>().As<IDbContextFactory>();

        builder.RegisterAssemblyTypes(ContainerBuilderExtension.Assemblies)
            .Where(t =>
            {
                var interfaces = t.GetInterfaces();
                var isContextFactory = interfaces.Any(it =>
                    it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IDesignTimeDbContextFactory<>));

                return isContextFactory;
            })
            .AsImplementedInterfaces()
            .As<IDesignTimeDbContextFactory<DbContext>>()
            .AsSelf();

        builder.RegisterType<MigrateContextJob>().AsSelf().AsImplementedInterfaces();
    }
}