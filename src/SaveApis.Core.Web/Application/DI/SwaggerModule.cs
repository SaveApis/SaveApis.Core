using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Core.Web.Infrastructure.DI;

namespace SaveApis.Core.Web.Application.DI;

public class SwaggerModule : BaseWebModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddSwaggerGen();

        builder.Populate(collection);
    }

    protected override void PreAuthentication(WebApplication application)
    {
        if (!application.Environment.IsDevelopment())
        {
            return;
        }

        application.UseSwagger();
        application.UseSwaggerUI();
    }
}
