using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveApis.Example.Domains.EfCore.Application.Models.Entities;
using SaveApis.Example.Domains.EfCore.Application.Models.VO;

namespace SaveApis.Example.Domains.EfCore.Persistence.Sql.Configurations;

public class TestEntityConfiguration : IEntityTypeConfiguration<TestEntity>
{
    public void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        builder.ToTable("test");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasConversion<TestId.EfCoreValueConverter>();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).IsRequired(false).HasMaxLength(200);

        builder.HasIndex(e => e.Name).IsUnique();

        var entity = TestEntity.Create("Test Entity", "Test Description");
        builder.HasData(entity);
    }
}
