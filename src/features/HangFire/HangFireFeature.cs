namespace HangFire;

using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Invocative.Neko.Framework.App;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

[PublicAPI]
public record HangFireFeature(Action<IGlobalConfiguration, IAppCreationContext> Config, IAppCreationContext Context) : AppFeature
{
    public override void BeforeRegistrations(WebApplicationBuilder webBuilder)
    {
        webBuilder.Services.AddHangfire(x => {
            x.UseConsole();
            Config(x, Context);
        });
        webBuilder.Services.AddHangfireServer();
    }

    public override void BeforeRun(WebApplication webBuilder)
    {
        webBuilder.UseHangfireDashboard(options: new DashboardOptions()
        {
            Authorization = new List<IDashboardAuthorizationFilter> {
                new LocalRequestsOnlyAuthorizationFilter(),
            }
        });
        base.BeforeRun(webBuilder);
    }
}
[PublicAPI]
public static class HangFireFeatureIAppBuilder
{
    public static IAppBuilder HangFire(this IAppBuilder builder, Action<IGlobalConfiguration, IAppCreationContext> config)
        => builder.InjectFeature(x => new HangFireFeature(config, x));
}
