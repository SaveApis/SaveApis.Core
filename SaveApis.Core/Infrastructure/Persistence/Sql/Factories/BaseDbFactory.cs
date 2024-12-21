using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace SaveApis.Core.Infrastructure.Persistence.Sql.Factories;

public abstract class BaseDbFactory<TContext>(IConfiguration configuration)
    : IDbFactory<TContext> where TContext : DbContext
{
    protected abstract TContext Create(DbContextOptionsBuilder<TContext> builder);

    public TContext CreateDbContext(string[] args)
    {
        var connectionString = BuildConnectionString();

        var builder = new DbContextOptionsBuilder<TContext>()
            .UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion, optionsBuilder =>
            {
                optionsBuilder.SchemaBehavior(MySqlSchemaBehavior.Translate,
                    (schema, table) => $"{schema.ToLowerInvariant()}_{table.ToLowerInvariant()}");
                optionsBuilder.EnableStringComparisonTranslations();
            });

        return Create(builder);
    }

    private string BuildConnectionString()
    {
        var name = configuration["db_mysql_name"] ?? "saveapis";
        var host = configuration["db_mysql_host"] ?? "localhost";
        var port = uint.TryParse(configuration["db_mysql_port"] ?? "3306", out var p) ? p : 3306;
        var user = configuration["db_mysql_user"] ?? "root";
        var password = configuration["db_mysql_password"] ?? "root";
        var database = configuration["db_mysql_database"] ?? "saveapis";

        var connectionStringBuilder = new MySqlConnectionStringBuilder()
        {
            Database = database,
            Pooling = true,
            Port = port,
            Server = host,
            Password = password,
            Pipelining = true,
            ApplicationName = name,
            UseCompression = true,
            AllowUserVariables = true,
            UserID = user,
            UseAffectedRows = true,
        };

        return connectionStringBuilder.ToString();
    }
}
