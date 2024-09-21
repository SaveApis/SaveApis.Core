﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Infrastructure.Persistence.MySql.Interfaces;

namespace SaveApis.Core.Application.DI;

public class EfCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder
        {
            Server = Environment.GetEnvironmentVariable("MYSQL_HOST"),
            Port = uint.Parse(Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306"),
            Database = Environment.GetEnvironmentVariable("MYSQL_DATABASE"),
            UserID = Environment.GetEnvironmentVariable("MYSQL_USER"),
            Password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD"),
            BrowsableConnectionString = false,
        };

        var assemblies = WebApplicationBuilderExtension.Assemblies;

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
            .As<DbContext>();
    }
}