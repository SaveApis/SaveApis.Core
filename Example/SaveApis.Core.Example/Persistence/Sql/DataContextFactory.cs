using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Persistence.MySql;

namespace SaveApis.Core.Example.Persistence.Sql;

public class DataContextFactory(IConfiguration configuration) : BaseDbContextFactory<DataContext>(configuration)
{
    protected override DataContext CreateContext(DbContextOptionsBuilder<DataContext> builder)
    {
        return new DataContext(builder.Options);
    }
}