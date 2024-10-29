using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Example.Domains.Models.Entity;
using SaveApis.Core.Infrastructure.Persistence.MySql.Interfaces;

namespace SaveApis.Core.Example.Persistence.MySQL.Interfaces;

public interface ITestDbContext : IDbContext
{
    public DbSet<TestEntity> TestEntities { get; set; }
}