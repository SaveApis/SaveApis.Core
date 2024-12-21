using Autofac;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Persistence.Sql.Factories;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace SaveApis.Core.Persistence.Sql.Manager;

public class DbManager(ILifetimeScope scope) : IDbManager
{
    public TContext Create<TContext>() where TContext : DbContext
    {
        var factory = scope.Resolve<IDbFactory<TContext>>();

        return factory.CreateDbContext([]);
    }
}
