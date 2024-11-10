using Microsoft.EntityFrameworkCore;

namespace SaveApis.Core.Infrastructure.Persistence.MySql.Interfaces;

public interface IDbContextFactory
{
    IReadOnlyCollection<DbContext> CreateAll();
    TContext Create<TContext>() where TContext : DbContext;
}