using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SaveApis.Core.Web.Application.Builder;
using SaveApis.Core.Web.Infrastructure.Builder;
using SaveApis.Core.Web.Infrastructure.DI;

namespace SaveApis.Core.Web.Application.DI;

public class JwtModule(IConfiguration configuration) : BaseWebModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var issuer = configuration["jwt_issuer"] ?? string.Empty;
        var audience = configuration["jwt_audience"] ?? string.Empty;
        var key = configuration["jwt_key"] ?? string.Empty;

        var collection = new ServiceCollection();

        collection.AddAuthorization();
        collection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
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

        builder.Populate(collection);

        builder.RegisterType<JwtBuilder>().As<IJwtBuilder>();
    }
}
