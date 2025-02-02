using Example.Web.Domains.EfCore.Domain.Entities;
using Example.Web.Domains.EfCore.Persistence.Sql.Configuration;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Common.Infrastructure.Persistence.Sql;

namespace Example.Web.Domains.EfCore.Persistence.Sql;

public class EfCoreDbContext(DbContextOptions options) : BaseDbContext(options)
{
    protected override string Schema => "EfCore";

    public DbSet<TestEntity> TestEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TestEntityConfiguration());
    }
}
