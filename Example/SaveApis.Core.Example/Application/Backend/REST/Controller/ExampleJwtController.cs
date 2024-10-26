using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveApis.Core.Application.Builders.Interfaces;

namespace SaveApis.Core.Example.Application.Backend.REST.Controller;

[ApiController]
[Route("jwt")]
public class ExampleJwtController(IJwtTokenBuilder builder) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("generate")]
    public async Task<IActionResult> Generate()
    {
        var token = await builder.WithClaim("test", "test").Build();
        return Ok(token);
    }

    [HttpGet("authorized")]
    public IActionResult Authorized()
    {
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("anonymous")]
    public IActionResult Anonymous()
    {
        return Ok();
    }
}