using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Core.Application.Events;

namespace SaveApis.Core.Application.Extensions;

public static class WebApplicationExtension
{
    public static async Task RunSaveApisAsync(this WebApplication application)
    {
        application.UseAuthentication();
        application.UseAuthorization();

        foreach (var action in WebApplicationBuilderExtension.PostActions) action(application);

        application.MapControllers().RequireAuthorization();

        var mediator = application.Services.CreateScope().ServiceProvider.GetRequiredService<IMediator>();
        await application.StartAsync();
        await mediator.Publish(new ApplicationStartedEvent());
        await application.WaitForShutdownAsync();
        await mediator.Publish(new ApplicationStoppedEvent());
    }
}