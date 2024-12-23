using HotChocolate.Authorization;
using HotChocolate.Types;

namespace SaveApis.Example.Domains.Jwt.Application.Backend;

[QueryType]
public static class JwtQuery
{
    public static string Anonym()
    {
        return "Anonym";
    }

    [Authorize]
    public static string Authenticated()
    {
        return "Authenticated";
    }

    [Authorize(Roles = ["administrator"])]
    public static string Administrator()
    {
        return "Administrator";
    }
}
