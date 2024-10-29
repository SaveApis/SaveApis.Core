using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SaveApis.Core.Application.Builders;
using SaveApis.Core.Application.Builders.Interfaces;
using SaveApis.Core.Domain.Settings;

namespace SaveApis.Core.Application.DI;

public class JwtModule(IConfiguration configuration) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        var issuer = configuration["JWT_ISSUER"]
#if !DEBUG
                     ?? "debug"
#else
                     ?? throw new ArgumentException("JWT_ISSUER")
#endif
            ;
        var audience = configuration["JWT_AUDIENCE"]
#if !DEBUG
                       ?? "debug"
#else
                       ?? throw new ArgumentException("JWT_AUDIENCE")
#endif
            ;

        var key = configuration["JWT_KEY"]
#if !DEBUG
                  ?? "yourRandomWith64OrMoreLengthKeyWhichShouldBeStoredSafetyAndShouldNotBeSharedWithOtherPeople"
#else
                  ?? throw new ArgumentException("JWT_KEY")
#endif
            ;
        var expirationInHours =
                uint.TryParse(configuration["JWT_EXPIRATION_IN_HOURS"], out var hours)
                    ? hours
#if !DEBUG
                    : 8
#else
                    : throw new ArgumentException("JWT_EXPIRATION_IN_HOURS")
#endif
            ;


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