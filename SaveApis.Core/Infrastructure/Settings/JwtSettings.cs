namespace SaveApis.Core.Infrastructure.Settings;

public record JwtSettings(string Issuer, string Audience, string Key, uint ExpirationInHours);