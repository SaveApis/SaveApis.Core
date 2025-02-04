using Autofac;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SaveApis.Core.Common.Application.Hangfire.Types;
using SaveApis.Core.Web.Infrastructure.DI;

namespace SaveApis.Core.Web.Application.DI;

public class HangfireDashboardModule(IConfiguration configuration) : BaseWebModule
{
    protected override void Load(ContainerBuilder builder)
    {
    }

    protected override void PostAuthentication(WebApplication application)
    {
        var hangfireType = Enum.TryParse(configuration["hangfire_server_type"], true, out HangfireServerType serverType) ? serverType : HangfireServerType.Server;
        if (hangfireType == HangfireServerType.Worker)
        {
            return;
        }

        var scope = application.Services.CreateScope();

        var filters = scope.ServiceProvider.GetRequiredService<IEnumerable<IDashboardAuthorizationFilter>>().ToList();
        var asyncFilters = scope.ServiceProvider.GetRequiredService<IEnumerable<IDashboardAsyncAuthorizationFilter>>().ToList();

        if (!application.Environment.IsDevelopment())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

            switch (filters.Count)
            {
                case 0 when asyncFilters.Count == 0:
                    logger.LogWarning("No Hangfire dashboard authorization filters are registered! The dashboard will be publicly accessible");

                    return;
                case > 0:
                    logger.LogWarning("There are synchronous Hangfire dashboard authorization filters registered! Consider using asynchronous filters to avoid blocking the dashboard");

                    break;
            }
        }

        application.MapHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = [.. filters],
            AsyncAuthorization = [.. asyncFilters],
            DarkModeEnabled = true,
            DisplayStorageConnectionString = true,
        });
    }
}
