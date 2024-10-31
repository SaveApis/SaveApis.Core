using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Application.Extensions;

namespace SaveApis.Core.Infrastructure.DI;

public abstract class BaseModule(IConfiguration configuration) : Module
{
    protected IConfiguration Configuration { get; } = configuration;

    protected override void Load(ContainerBuilder builder)
    {
        WebApplicationBuilderExtension.PostActions.Add(PostAction);
        Register(builder);
    }

    protected virtual void PostAction(WebApplication application)
    {
    }

    protected abstract void Register(ContainerBuilder builder);
}