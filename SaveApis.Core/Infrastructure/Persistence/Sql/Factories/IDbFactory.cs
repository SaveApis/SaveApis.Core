using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SaveApis.Core.Infrastructure.Persistence.Sql.Factories;

public interface IDbFactory<out TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext;
