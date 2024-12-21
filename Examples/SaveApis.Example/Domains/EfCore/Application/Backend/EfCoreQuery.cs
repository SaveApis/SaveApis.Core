using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;
using SaveApis.Example.Domains.EfCore.Application.Mapper;
using SaveApis.Example.Domains.EfCore.Application.Models.Dto;
using SaveApis.Example.Domains.EfCore.Application.Models.Entities;
using SaveApis.Example.Domains.EfCore.Persistence.Sql;

namespace SaveApis.Example.Domains.EfCore.Application.Backend;

[QueryType]
public static class EfCoreQuery
{
    public static async Task<ICollection<TestGetDto>> TestEntities([Service] IDbManager manager)
    {
        var mapper = new TestEntityMapper();
        var context = manager.Create<TestContext>();

        var list = await context.TestEntities.ToListAsync().ConfigureAwait(false);

        return list.ConvertAll(mapper.EntityToDto);
    }
}
