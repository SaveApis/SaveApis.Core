using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Domains.Logging.Domain.Entities;

namespace SaveApis.Core.Common.Domains.Logging.Persistence.Sql.Configurations;

public class LogEntryEntityConfiguration : IEntityTypeConfiguration<LogEntryEntity>
{
    public void Configure(EntityTypeBuilder<LogEntryEntity> builder)
    {
        builder.ToTable("Entries");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(e => e.LoggedAt).IsRequired();
        builder.Property(e => e.LogType).IsRequired().HasConversion<string>();

        builder.Property(e => e.AffectedEntityId).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(e => e.AffectedEntityName).IsRequired().HasConversion<Name.EfCoreValueConverter>();
        builder.Property(e => e.State).IsRequired().HasConversion<string>();

        builder.Property(e => e.LoggedBy).IsRequired(false).HasConversion<Id.EfCoreValueConverter>();

        builder.HasMany(e => e.Values).WithOne().HasForeignKey(v => v.LogEntryId);

        builder.HasIndex(e => e.AffectedEntityId);
        builder.HasIndex(e => e.LogType);
        builder.HasIndex(e => e.State);
    }
}
