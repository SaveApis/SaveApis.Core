namespace SaveApis.Core.Infrastructure.Jwt.Builder;

public interface IJwtBuilder : IBuilder<string>
{
    IJwtBuilder WithClaim(string type, string value);

    IJwtBuilder WithRole(string role);
    IJwtBuilder WithRoles(params string[] roles);
}
