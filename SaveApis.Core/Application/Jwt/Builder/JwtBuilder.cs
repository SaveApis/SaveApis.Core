using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SaveApis.Core.Infrastructure.Jwt.Builder;

namespace SaveApis.Core.Application.Jwt.Builder;

public class JwtBuilder(IConfiguration configuration) : IJwtBuilder
{
    private readonly Dictionary<string, string> _claims = [];
    private readonly ICollection<string> _roles = [];
    public IJwtBuilder WithClaim(string type, string value)
    {
        _claims.Add(type, value);

        return this;
    }

    public IJwtBuilder WithRole(string role)
    {
        _roles.Add(role);

        return this;
    }

    public IJwtBuilder WithRoles(params string[] roles)
    {
        foreach (var role in roles)
        {
            _roles.Add(role);
        }

        return this;
    }

    public string Build()
    {
        var key = configuration["jwt_key"] ?? throw new SecurityTokenValidationException("JWT key is missing!");
        var issuer = configuration["jwt_issuer"] ?? throw new SecurityTokenValidationException("JWT issuer is missing!");
        var audience = configuration["jwt_audience"] ?? throw new SecurityTokenValidationException("JWT audience is missing!");
        var expirationTime = configuration["jwt_expiration"] ?? throw new SecurityTokenValidationException("JWT expiration time is missing!");

        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha512);
        var descriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Audience = audience,
            Issuer = issuer,
            Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(expirationTime, CultureInfo.InvariantCulture)),
            Subject = GenerateClaims(),
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    public Task<string> BuildAsync()
    {
        return Task.FromResult(Build());
    }

    private ClaimsIdentity GenerateClaims()
    {
        var claimsIdentity = new ClaimsIdentity();

        foreach (var claim in _claims)
        {
            claimsIdentity.AddClaim(new Claim(claim.Key, claim.Value));
        }

        foreach (var role in _roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return claimsIdentity;
    }
}
