using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PasswordGenerator;
using SaveApis.Core.Application.Jwt.Builder;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Jwt.Builder;

namespace SaveApis.Core.Application.DI;

public class JwtModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Load(ContainerBuilder builder)
    {
        var issuer = Configuration["jwt_issuer"] ?? "saveapis";
        var audience = Configuration["jwt_audience"] ?? "saveapis";
        var key = Configuration["jwt_key"] ?? new Password(true, true, true, true, 64).Next();

        var services = new ServiceCollection();

        services.AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                };
            });

        builder.Populate(services);

        builder.RegisterType<JwtBuilder>().As<IJwtBuilder>();
    }

    public override void PostAction(WebApplication application)
    {
        application.UseAuthentication();
        application.UseAuthorization();
    }
}
