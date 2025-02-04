using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveApis.Core.Web.Infrastructure.Builder;

namespace Example.Web.Domains.Jwt.Application.Backend.REST;

[ApiController]
[Route("api/jwt")]
public class JwtController(IJwtBuilder builder) : ControllerBase
{
    [HttpGet("unauthorized")]
    public IActionResult GetJwt()
    {
        var jwt = builder.Build();

        return Ok(jwt);
    }

    [HttpGet("authorized")]
    [Authorize]
    public IActionResult GetJwtWithAuth()
    {
        var jwt = builder.Build();

        return Ok(jwt);
    }
}
