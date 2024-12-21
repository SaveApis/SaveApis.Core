using Microsoft.EntityFrameworkCore;

namespace SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

public interface IDbManager
{
    TContext Create<TContext>() where TContext : DbContext;
}
