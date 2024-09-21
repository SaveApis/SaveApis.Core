using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Example.Application.Models.Entity;
using SaveApis.Core.Example.Persistence.MySQL.Interfaces;

namespace SaveApis.Core.Example.Persistence.MySQL;

public class TestDbContext(DbContextOptions options) : DbContext(options), ITestDbContext
{
    public DbSet<TestEntity> TestEntities { get; set; }
}