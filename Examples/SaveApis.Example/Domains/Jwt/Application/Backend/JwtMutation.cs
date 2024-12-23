using HotChocolate;
using HotChocolate.Types;
using SaveApis.Core.Infrastructure.Jwt.Builder;

namespace SaveApis.Example.Domains.Jwt.Application.Backend;

[MutationType]
public static class JwtMutation
{
    public static string AdministratorToken([Service] IJwtBuilder builder)
    {
        return builder.WithClaim("test", "test").WithRole("administrator").Build();
    }

    public static string ManagerToken([Service] IJwtBuilder builder)
    {
        return builder.WithClaim("test", "test").WithRole("manager").Build();
    }

    public static string UserToken([Service] IJwtBuilder builder)
    {
        return builder.WithClaim("test", "test").WithRole("user").Build();
    }
}
