using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SaveApis.Core.Common.Application.Exceptions;

namespace SaveApis.Core.Common.Infrastructure.Persistence.Sql.Factories;

public class BaseDesignTimeDbContextFactory<TContext>(IConfiguration configuration) : IDesignTimeDbContextFactory<TContext> where TContext : BaseDbContext
{
    public TContext CreateDbContext(string[] args)
    {
        var options = BuildOptions();

        var instance = Activator.CreateInstance(typeof(TContext), options);
        if (instance is not TContext context)
        {
            throw new InvalidDbContextException<TContext>();
        }

        return context;
    }
    private DbContextOptions<TContext> BuildOptions()
    {
        var connectionString = BuildConnectionString();

        var builder = new DbContextOptionsBuilder<TContext>();
        builder.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion, optionsBuilder =>
        {
            optionsBuilder.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}");
            optionsBuilder.EnableStringComparisonTranslations();
            optionsBuilder.EnableIndexOptimizedBooleanColumns();
            optionsBuilder.UseNewtonsoftJson();
        });

        return builder.Options;
    }
    private string BuildConnectionString()
    {
        var name = configuration["database_sql_name"] ?? string.Empty;
        var server = configuration["database_sql_server"] ?? string.Empty;
        var port = configuration["database_sql_port"] ?? string.Empty;
        var database = configuration["database_sql_database"] ?? string.Empty;
        var user = configuration["database_sql_user"] ?? string.Empty;
        var password = configuration["database_sql_password"] ?? string.Empty;

        var builder = new MySqlConnectionStringBuilder
        {
            ApplicationName = name,
            Server = server,
            Port = uint.TryParse(port, out var p) ? p : 3306,
            Database = database,
            UserID = user,
            Password = password,
            Pooling = true,
            Pipelining = true,
            UseCompression = true,
            AllowUserVariables = true,
            BrowsableConnectionString = false,
            UseAffectedRows = true,
        };

        return builder.ToString();
    }
}
