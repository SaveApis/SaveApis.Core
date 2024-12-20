using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace SaveApis.Core.Infrastructure.DI;

public class BaseModule(IConfiguration configuration) : Module
{
    protected IConfiguration Configuration { get; } = configuration;

    public virtual void PostAction(WebApplication application)
    {
    }

    public virtual Task PostActionAsync(WebApplication application)
    {
        return Task.CompletedTask;
    }
}
