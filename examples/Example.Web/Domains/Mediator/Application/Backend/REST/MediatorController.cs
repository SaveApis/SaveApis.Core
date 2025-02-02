using Example.Web.Domains.Mediator.Application.Mediator.Commands.Test;
using Example.Web.Domains.Mediator.Application.Mediator.Queries.Test;
using FluentResults.Extensions.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Example.Web.Domains.Mediator.Application.Backend.REST;

[ApiController]
[Route("api/mediator")]
public class MediatorController(IMediator mediator) : ControllerBase
{
    [HttpGet("query")]
    public async Task<IActionResult> Query()
    {
        var result = await mediator.Send(new TestQuery()).ConfigureAwait(false);

        return result.ToActionResult();
    }

    [HttpPost("command")]
    public async Task<IActionResult> Command()
    {
        var result = await mediator.Send(new TestCommand()).ConfigureAwait(false);

        return result.ToActionResult();
    }
}
