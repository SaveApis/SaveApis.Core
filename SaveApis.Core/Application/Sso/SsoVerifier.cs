using Microsoft.Extensions.Configuration;
using SaveApis.Core.Infrastructure.Sso;
using SaveApis.SSO.Client;
using StrawberryShake;

namespace SaveApis.Core.Application.Sso;

public class SsoVerifier(IConfiguration configuration, ISsoClient client) : ISsoVerifier
{
    public async Task<bool> VerifyProjectAsync(string userName, string password)
    {
        var projectId = configuration["sso_project"] ?? "savapis";

        var projectLogin = new ProjectLoginDtoInput
        {
            Id = projectId,
            Password = password,
            UserName = userName,
        };
        var result = await client.ProjectLogin.ExecuteAsync(projectLogin).ConfigureAwait(false);
        result.EnsureNoErrors();

        return result.Data?.ProjectLogin ?? false;
    }
}
