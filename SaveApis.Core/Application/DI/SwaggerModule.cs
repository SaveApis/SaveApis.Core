using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SaveApis.Core.Infrastructure.DI;

namespace SaveApis.Core.Application.DI;

public class SwaggerModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddEndpointsApiExplorer();
        collection.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    []
                }
            });
        });

        builder.Populate(collection);
    }

    protected override void PostAction(WebApplication application)
    {
        if (!application.Environment.IsDevelopment() && !application.Environment.IsStaging()) return;
        application.UseSwagger();
        application.UseSwaggerUI();
    }
}