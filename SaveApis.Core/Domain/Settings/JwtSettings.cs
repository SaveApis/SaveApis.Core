namespace SaveApis.Core.Domain.Settings;

public record JwtSettings(string Issuer, string Audience, string Key, uint ExpirationInHours);