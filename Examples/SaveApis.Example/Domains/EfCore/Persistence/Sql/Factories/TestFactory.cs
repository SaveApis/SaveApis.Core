using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Persistence.Sql.Factories;

namespace SaveApis.Example.Domains.EfCore.Persistence.Sql.Factories;

public class TestFactory(IConfiguration configuration) : BaseDbFactory<TestContext>(configuration)
{
    // Required for Migrations
    public TestFactory() : this(new ConfigurationBuilder().AddInMemoryCollection().Build())
    {
    }

    protected override TestContext Create(DbContextOptionsBuilder<TestContext> builder)
    {
        return new TestContext(builder.Options);
    }
}
