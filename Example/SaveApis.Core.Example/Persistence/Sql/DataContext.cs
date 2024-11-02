using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Example.Domain.Models.Sql;
using SaveApis.Core.Example.Persistence.Sql.Configurations;
using SaveApis.Core.Infrastructure.Persistence.MySql.Interfaces;

namespace SaveApis.Core.Example.Persistence.Sql;

public class DataContext(DbContextOptions options) : DbContext(options), IDbContext
{
    public DbSet<SqlEntity> SqlEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SqlEntityConfiguration());
    }
}