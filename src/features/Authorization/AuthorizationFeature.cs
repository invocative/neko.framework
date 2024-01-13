namespace Invocative.Neko.Feature.Authorization;

using Framework.App;
using Microsoft.AspNetCore.Builder;

public record AuthorizationFeature : AppFeature
{
    public override void AfterRegistration(WebApplicationBuilder webBuilder)
        => webBuilder.AddJwt();

    public override void BeforeRun(WebApplication webBuilder)
    {
        webBuilder.UseAuthentication();
        webBuilder.UseAuthorization();
        webBuilder.UseMiddleware<RoleBasedPermissionMiddleware>();
    }
}
