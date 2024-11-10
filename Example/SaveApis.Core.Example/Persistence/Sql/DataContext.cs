using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Example.Domain.Models.Sql;
using SaveApis.Core.Example.Persistence.Sql.Configurations;

namespace SaveApis.Core.Example.Persistence.Sql;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<SqlEntity> SqlEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SqlEntityConfiguration());
    }
}