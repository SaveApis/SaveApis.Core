using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SaveApis.Core.Infrastructure.Persistence.MySql.Interfaces;

namespace SaveApis.Core.Persistence.MySql;

public class DbContextFactory(ILifetimeScope scope) : IDbContextFactory
{
    public IReadOnlyCollection<DbContext> CreateAll()
    {
        var dbContexts = scope.Resolve<IEnumerable<IDesignTimeDbContextFactory<DbContext>>>();
        return dbContexts.Select(factory => factory.CreateDbContext([])).ToList();
    }

    public TContext Create<TContext>() where TContext : DbContext
    {
        var factory = scope.Resolve<IDesignTimeDbContextFactory<TContext>>();
        return factory.CreateDbContext([]);
    }
}