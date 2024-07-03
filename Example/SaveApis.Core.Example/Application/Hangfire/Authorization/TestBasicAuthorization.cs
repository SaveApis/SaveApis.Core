using System.Security.Claims;
using System.Text;
using Hangfire.Dashboard;

namespace SaveApis.Core.Example.Application.Hangfire.Authorization;

/// <summary>
/// If you want to test this you have to set the environment variable ASPNETCORE_ENVIRONMENT TO `Staging` or `Production` in docker-compose.yml
/// </summary>
public class TestBasicAuthorization : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        // Check if user is authenticated
        if (httpContext.User.Identity.IsAuthenticated)
        {
            return true;
        }

        // Otherwise, perform basic authentication
        var header = httpContext.Request.Headers.Authorization;
        if (string.IsNullOrWhiteSpace(header))
        {
            httpContext.Response.Headers.WWWAuthenticate = "Basic realm=\"Hangfire Dashboard\"";
            httpContext.Response.StatusCode = 401;
            return false;
        }

        var authValues = Encoding.UTF8.GetString(Convert.FromBase64String(header.ToString().Substring(6))).Split(':');
        var username = authValues[0];
        var password = authValues[1];

        // Validate credentials (replace with your logic)
        if (username == "admin" && password == "password")
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "Basic");
            httpContext.User = new ClaimsPrincipal(identity);
            return true;
        }

        httpContext.Response.StatusCode = 401;
        return false;
    }
}