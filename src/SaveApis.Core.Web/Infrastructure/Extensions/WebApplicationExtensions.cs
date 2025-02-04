using Microsoft.AspNetCore.Builder;

namespace SaveApis.Core.Web.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static async Task RunSaveApisAsync(this WebApplication application)
    {
        foreach (var action in ContainerBuilderExtensions.PreAuthenticationActions)
        {
            await action(application).ConfigureAwait(false);
        }

        application.UseAuthentication();
        application.UseAuthorization();

        foreach (var action in ContainerBuilderExtensions.PostAuthenticationActions)
        {
            await action(application).ConfigureAwait(false);
        }

        await application.RunAsync().ConfigureAwait(false);
    }
}
