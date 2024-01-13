namespace Logging;

using Invocative.Neko.Framework.App;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

[PublicAPI]
public record LoggingFeature() : AppFeature
{
    public override void BeforeRegistrations(WebApplicationBuilder webBuilder)
    {
        webBuilder.Logging.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.IncludeScopes = false;
        });
    }
}


[PublicAPI]
public static class LoggingFeatureIAppBuilder
{
    public static IAppBuilder Logging(this IAppBuilder builder)
        => builder.InjectFeature(() => new LoggingFeature());
}
