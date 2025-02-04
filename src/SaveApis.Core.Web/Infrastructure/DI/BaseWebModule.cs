using Microsoft.AspNetCore.Builder;
using SaveApis.Core.Common.Infrastructure.DI;

namespace SaveApis.Core.Web.Infrastructure.DI;

public abstract class BaseWebModule : BaseModule
{
    /// <summary>
    /// Action method executed before the authentication process
    /// </summary>
    /// <param name="application">Current application</param>
    protected virtual void PreAuthentication(WebApplication application)
    {
    }

    /// <summary>
    /// Async action method executed before the authentication process
    /// </summary>
    /// <param name="application">Current application</param>
    /// <returns><see cref="Task"/></returns>
    public virtual Task PreAuthenticationAsync(WebApplication application)
    {
        PreAuthentication(application);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Action method executed after the authentication process
    /// </summary>
    /// <param name="application">Current application</param>
    protected virtual void PostAuthentication(WebApplication application)
    {
    }

    /// <summary>
    /// Async action method executed after the authentication process
    /// </summary>
    /// <param name="application">Current application</param>
    /// <returns><see cref="Task"/></returns>
    public virtual Task PostAuthenticationAsync(WebApplication application)
    {
        PostAuthentication(application);

        return Task.CompletedTask;
    }
}
