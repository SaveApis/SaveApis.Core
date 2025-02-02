using System.Security.Claims;
using SaveApis.Core.Common.Infrastructure.Builder;

namespace SaveApis.Core.Web.Infrastructure.Builder;

/// <summary>
/// Interface for JWT builder
/// </summary>
public interface IJwtBuilder : IBuilder<string>
{
    /// <summary>
    /// Add a claim to the JWT
    /// </summary>
    /// <param name="type">Type of the claim</param>
    /// <param name="value">Value of the claim</param>
    /// <returns>Current instance of the Builder</returns>
    IJwtBuilder AddClaim(string type, string value);

    /// <summary>
    /// Add a claim to the JWT
    /// </summary>
    /// <param name="claim">One <see cref="Claim">claim</see></param>
    /// <returns>Current instance of the Builder</returns>
    IJwtBuilder AddClaim(Claim claim);

    /// <summary>
    /// Add multiple claims to the JWT
    /// </summary>
    /// <param name="claims">List of <see cref="Claim">claims</see></param>
    /// <returns>Current instance of the Builder</returns>
    IJwtBuilder AddClaims(params Claim[] claims);
}
