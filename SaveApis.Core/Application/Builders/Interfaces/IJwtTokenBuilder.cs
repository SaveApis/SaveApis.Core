using SaveApis.Core.Infrastructure.Builders.Interfaces;

namespace SaveApis.Core.Application.Builders.Interfaces;

public interface IJwtTokenBuilder : IBuilder<string>
{
    IJwtTokenBuilder WithClaim(string type, string value);
}