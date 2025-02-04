using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SaveApis.Core.Web.Infrastructure.Builder;

namespace SaveApis.Core.Web.Application.Builder;

public class JwtBuilder(IConfiguration configuration) : IJwtBuilder
{
    private ICollection<Claim> Claims { get; } = [];

    public IJwtBuilder AddClaim(string type, string value)
    {
        AddClaim(new Claim(type, value));

        return this;
    }

    public IJwtBuilder AddClaim(Claim claim)
    {
        Claims.Add(claim);

        return this;
    }

    public IJwtBuilder AddClaims(params Claim[] claims)
    {
        foreach (var claim in claims)
        {
            AddClaim(claim);
        }

        return this;
    }

    public string Build()
    {
        var now = DateTime.UtcNow;
        var issuer = configuration["jwt_issuer"] ?? string.Empty;
        var audience = configuration["jwt_audience"] ?? string.Empty;
        var key = Encoding.UTF8.GetBytes(configuration["jwt_key"] ?? string.Empty);
        var expiration = configuration["jwt_expiration"] ?? string.Empty;

        var handler = new JwtSecurityTokenHandler();
        var descriptor = new SecurityTokenDescriptor
        {
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(Claims),
            Expires = now.AddHours(int.TryParse(expiration, out var hours) ? hours : 1),
            NotBefore = now,
            IssuedAt = now,
        };

        var token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    public Task<string> BuildAsync()
    {
        return Task.FromResult(Build());
    }
}
