using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Application.Jobs.MySql;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Persistence.MySql.Interfaces;

namespace SaveApis.Core.Application.DI;

public class EfCoreModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder
        {
            Server = Configuration["MYSQL_HOST"] ?? "localhost",
            Port = uint.Parse(Configuration["MYSQL_PORT"] ?? "3306"),
            Database = Configuration["MYSQL_DATABASE"] ?? "SaveApis",
            UserID = Configuration["MYSQL_USER"] ?? "saveapis",
            Password = Configuration["MYSQL_PASSWORD"] ?? "saveapis",
            BrowsableConnectionString = false,
        };

        var assemblies = ContainerBuilderExtension.Assemblies;

        var collection = new ServiceCollection();

        var dbContextTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsAssignableTo(typeof(IDbContext)) && t is { IsClass: true, IsAbstract: false });

        foreach (var dbContextType in dbContextTypes)
        {
            var method = typeof(EntityFrameworkServiceCollectionExtensions).GetMethods().First(m =>
                m.Name == nameof(EntityFrameworkServiceCollectionExtensions.AddDbContext) &&
                m.GetGenericArguments().Length == 1);

            var genericMethod = method.MakeGenericMethod(dbContextType);

            genericMethod.Invoke(null,
            [
                collection,
                new Action<DbContextOptionsBuilder>(options =>
                    options.UseMySql(connectionStringBuilder.ToString(),
                        MySqlServerVersion.LatestSupportedServerVersion).UseLazyLoadingProxies()),
                ServiceLifetime.Scoped,
                ServiceLifetime.Scoped
            ]);
        }

        builder.Populate(collection);

        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.IsAssignableTo(typeof(IDbContext)))
            .AsImplementedInterfaces()
            .As<DbContext>()
            .AsSelf();

        builder.RegisterType<MigrateContextJob>().AsImplementedInterfaces();
    }
}