using Microsoft.EntityFrameworkCore;

namespace SaveApis.Core.Infrastructure.Persistence.Sql;

public abstract class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    protected abstract string Schema { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema);
    }
}
