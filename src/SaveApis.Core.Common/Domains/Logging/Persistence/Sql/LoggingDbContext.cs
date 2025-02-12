using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Common.Domains.Logging.Domain.Entities;
using SaveApis.Core.Common.Domains.Logging.Persistence.Sql.Configurations;
using SaveApis.Core.Common.Infrastructure.Persistence.Sql;

namespace SaveApis.Core.Common.Domains.Logging.Persistence.Sql;

public class LoggingDbContext(DbContextOptions options) : BaseDbContext(options)
{
    protected override string Schema => "Logging";

    public DbSet<LogEntryEntity> LogEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new LogEntryEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LogEntryValueEntityConfiguration());
    }
}
