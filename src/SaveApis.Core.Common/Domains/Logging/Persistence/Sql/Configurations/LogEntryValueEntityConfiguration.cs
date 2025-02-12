using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Domains.Logging.Domain.Entities;

namespace SaveApis.Core.Common.Domains.Logging.Persistence.Sql.Configurations;

public class LogEntryValueEntityConfiguration : IEntityTypeConfiguration<LogEntryValueEntity>
{
    public void Configure(EntityTypeBuilder<LogEntryValueEntity> builder)
    {
        builder.ToTable("Values");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(e => e.AttributeName).IsRequired().HasConversion<Name.EfCoreValueConverter>();
        builder.Property(e => e.OldValue).IsRequired(false).HasMaxLength(500);
        builder.Property(e => e.NewValue).IsRequired(false).HasMaxLength(500);

        builder.Property(e => e.LogEntryId).IsRequired().HasConversion<Id.EfCoreValueConverter>();

        builder.HasIndex(e => new { e.LogEntryId, e.AttributeName }).IsUnique();
    }
}
