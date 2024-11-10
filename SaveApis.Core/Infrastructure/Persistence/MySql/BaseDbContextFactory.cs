using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace SaveApis.Core.Infrastructure.Persistence.MySql;

public abstract class BaseDbContextFactory<TContext>(IConfiguration configuration) : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
{
    public TContext CreateDbContext(string[] args)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder
        {
            Server = configuration["MYSQL_HOST"] ?? "localhost",
            Port = uint.Parse(configuration["MYSQL_PORT"] ?? "3306"),
            Database = configuration["MYSQL_DATABASE"] ?? "SaveApis",
            UserID = configuration["MYSQL_USER"] ?? "saveapis",
            Password = configuration["MYSQL_PASSWORD"] ?? "saveapis",
            BrowsableConnectionString = false,
            AllowUserVariables = true
        };

        var builder = new DbContextOptionsBuilder<TContext>().UseMySql(connectionStringBuilder.ToString(),
            MySqlServerVersion.LatestSupportedServerVersion);

        return CreateContext(builder);
    }

    protected abstract TContext CreateContext(DbContextOptionsBuilder<TContext> builder);
}