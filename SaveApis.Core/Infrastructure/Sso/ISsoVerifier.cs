namespace SaveApis.Core.Infrastructure.Sso;

public interface ISsoVerifier
{
    Task<bool> VerifyProjectAsync(string userName, string password);
}
