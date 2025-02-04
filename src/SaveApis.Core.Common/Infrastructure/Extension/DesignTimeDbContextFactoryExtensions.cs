using Microsoft.EntityFrameworkCore.Design;
using SaveApis.Core.Common.Infrastructure.Persistence.Sql;

namespace SaveApis.Core.Common.Infrastructure.Extension;

public static class DesignTimeDbContextFactoryExtensions
{
    public static TContext Create<TContext>(this IDesignTimeDbContextFactory<TContext> factory) where TContext : BaseDbContext
    {
        return factory.CreateDbContext([]);
    }
}
