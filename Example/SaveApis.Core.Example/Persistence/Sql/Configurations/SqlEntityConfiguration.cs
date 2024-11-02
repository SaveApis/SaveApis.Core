using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveApis.Core.Example.Domain.Models.Sql;

namespace SaveApis.Core.Example.Persistence.Sql.Configurations;

public class SqlEntityConfiguration : IEntityTypeConfiguration<SqlEntity>
{
    public void Configure(EntityTypeBuilder<SqlEntity> builder)
    {
        builder.ToTable("SqlEntities");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Age).IsRequired();
    }
}