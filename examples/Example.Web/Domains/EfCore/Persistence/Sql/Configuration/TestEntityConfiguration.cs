using Example.Web.Domains.EfCore.Domain.Entities;
using Example.Web.Domains.EfCore.Domain.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example.Web.Domains.EfCore.Persistence.Sql.Configuration;

public class TestEntityConfiguration : IEntityTypeConfiguration<TestEntity>
{
    public void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        builder.ToTable("TestEntities");

        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(entity => entity.CreatedAt).IsRequired();
        builder.Property(entity => entity.UpdatedAt).IsRequired(false);

        builder.Property(entity => entity.FirstName).IsRequired().HasConversion<Name.EfCoreValueConverter>();
        builder.Property(entity => entity.LastName).IsRequired().HasConversion<Name.EfCoreValueConverter>();

        builder.HasData(TestEntity.Create(Name.From("Test"), Name.From("Test")));
    }
}
