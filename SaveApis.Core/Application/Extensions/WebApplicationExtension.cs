using Hangfire;
using Hangfire.Dashboard;
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
        var dashboardAuthorizationFilters = application.Services.CreateScope().ServiceProvider
            .GetServices<IDashboardAuthorizationFilter>().ToArray();
        var options = new DashboardOptions
        {
            Authorization = application.Environment.IsDevelopment() ? [] : dashboardAuthorizationFilters
        };
        application.UseHangfireDashboard("/hangfire", options);

        if (application.Environment.IsDevelopment())
        {
            application.UseSwagger();
            application.UseSwaggerUI();
        }

        application.UseAuthentication();
        application.UseAuthorization();

        application.MapControllers().RequireAuthorization();
        application.MapGraphQL();

        var mediator = application.Services.CreateScope().ServiceProvider.GetRequiredService<IMediator>();
        await application.StartAsync();
        await mediator.Publish(new ApplicationStartedEvent());
        await application.WaitForShutdownAsync();
        await mediator.Publish(new ApplicationStoppedEvent());
    }
}