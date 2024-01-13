namespace Invocative.Neko.Framework.Utils;

using System.Security.Claims;

public static class ClaimExtensions
{
    public static string? GetClaim(this ClaimsIdentity identity, string type)
    {
        if (identity is null)
            throw new ArgumentNullException(nameof(identity));

        if (string.IsNullOrEmpty(type))
            throw new ArgumentException(nameof(type));

        return identity.FindFirst(type)?.Value;
    }

    public static string? GetClaim(this ClaimsPrincipal principal, string type)
    {
        if (principal is null)
            throw new ArgumentNullException(nameof(principal));

        if (string.IsNullOrEmpty(type))
            throw new ArgumentException(nameof(type));

        return principal.FindFirst(type)?.Value;
    }
}
