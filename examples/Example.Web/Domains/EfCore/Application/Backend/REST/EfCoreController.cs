using Example.Web.Domains.EfCore.Persistence.Sql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Example.Web.Domains.EfCore.Application.Backend.REST;

[ApiController]
[Route("api/efcore")]
public class EfCoreController(IEfCoreDbContextFactory factory) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Query()
    {
        await using var context = factory.Create();

        var entities = await context.TestEntities.ToListAsync().ConfigureAwait(false);

        return Ok(entities);
    }
}
