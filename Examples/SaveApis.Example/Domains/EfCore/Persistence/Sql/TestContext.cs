using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Persistence.Sql;
using SaveApis.Example.Domains.EfCore.Application.Models.Entities;
using SaveApis.Example.Domains.EfCore.Persistence.Sql.Configurations;

namespace SaveApis.Example.Domains.EfCore.Persistence.Sql;

public class TestContext(DbContextOptions options) : BaseDbContext(options)
{
    protected override string Schema => "test";

    public DbSet<TestEntity> TestEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TestEntityConfiguration());
    }
}
