using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SaveApis.Core.Application.Builders.Interfaces;
using SaveApis.Core.Infrastructure.Settings;

namespace SaveApis.Core.Application.Builders;

public class JwtTokenBuilder(IOptions<JwtSettings> jwtOptions) : IJwtTokenBuilder
{
    private Dictionary<string, string> Claims { get; } = [];

    public IJwtTokenBuilder WithClaim(string type, string value)
    {
        Claims[type] = value;
        return this;
    }

    public Task<string> Build()
    {
        var settings = jwtOptions.Value;

        var handler = new JwtSecurityTokenHandler();
        var descriptor = new SecurityTokenDescriptor
        {
            Audience = settings.Audience,
            Issuer = settings.Issuer,
            Expires = DateTime.UtcNow.AddHours(settings.ExpirationInHours),
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
                SecurityAlgorithms.HmacSha512Signature),
            Subject = new ClaimsIdentity(Claims.Select(it => new Claim(it.Key, it.Value)))
        };

        var token = handler.CreateToken(descriptor);

        return Task.FromResult(handler.WriteToken(token));
    }
}