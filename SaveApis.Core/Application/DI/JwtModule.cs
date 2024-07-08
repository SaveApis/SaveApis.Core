using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SaveApis.Core.Application.Builders;
using SaveApis.Core.Application.Builders.Interfaces;
using SaveApis.Core.Infrastructure.Builders;
using SaveApis.Core.Infrastructure.Settings;

namespace SaveApis.Core.Application.DI;

public class JwtModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new ArgumentNullException("JWT_ISSUER");
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new ArgumentNullException("JWT_AUDIENCE");
        var key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new ArgumentNullException("JWT_KEY");
        var expirationInHours =
            uint.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRATION_IN_HOURS"), out var hours)
                ? hours
                : throw new ArgumentNullException("JWT_EXPIRATION_IN_HOURS");

        var jwtSettings = new JwtSettings(issuer, audience, key, expirationInHours);

        collection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ClockSkew = TimeSpan.Zero,
                };
            });

        builder.Populate(collection);

        builder.RegisterType<JwtTokenBuilder>().As<IJwtTokenBuilder>();

        builder.Register(_ => Options.Create(jwtSettings)).As<IOptions<JwtSettings>>();
    }
}