using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PasswordGenerator;
using SaveApis.Core.Application.Builders;
using SaveApis.Core.Application.Builders.Interfaces;
using SaveApis.Core.Domain.Settings;
using SaveApis.Core.Infrastructure.DI;

namespace SaveApis.Core.Application.DI;

public class JwtModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        var issuer = Configuration["JWT_ISSUER"]
#if !DEBUG
                     ?? "debug"
#else
                     ?? "http://localhost"
#endif
            ;
        var audience = Configuration["JWT_AUDIENCE"]
#if !DEBUG
                       ?? "debug"
#else
                       ?? "http://localhost"
#endif
            ;

        var key = Configuration["JWT_KEY"]
#if !DEBUG
                  ?? "yourRandomWith64OrMoreLengthKeyWhichShouldBeStoredSafetyAndShouldNotBeSharedWithOtherPeople"
#else
                  ?? new Password(true, true, true, true, 64).Next();
#endif
            ;
        var expirationInHours =
                uint.TryParse(Configuration["JWT_EXPIRATION_IN_HOURS"], out var hours)
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