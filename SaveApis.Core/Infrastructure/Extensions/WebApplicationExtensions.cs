using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace SaveApis.Core.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static async Task RunSaveApisAsync(this WebApplication application, string[] args)
    {
        foreach (var action in ContainerBuilderExtensions.Actions)
        {
            action(application);
        }

        foreach (var action in ContainerBuilderExtensions.AsyncActions)
        {
            await action(application).ConfigureAwait(false);
        }

        application.MapGraphQL();

        await application.RunWithGraphQLCommandsAsync(args).ConfigureAwait(false);
    }
}
