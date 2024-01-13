namespace Invocative.Neko.Framework.Features;

using App;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[PublicAPI]
public record DataProtectionLayerFeature(Action<IDataProtectionBuilder> Config) : AppFeature
{
    public override void BeforeRegistrations(WebApplicationBuilder webBuilder) 
        => Config(webBuilder.Services.AddDataProtection());
}


[PublicAPI]
public static class DataProtectionLayerFeatureIAppBuilder
{
    public static IAppBuilder DataProtection(this IAppBuilder builder, Action<IDataProtectionBuilder> config)
        => builder.InjectFeature(() => new DataProtectionLayerFeature(config));
}


[PublicAPI]
public record StaticFilesFeature(StaticFileOptions Config) : AppFeature
{
    public override void BeforeRun(WebApplication webBuilder) 
        => webBuilder.UseStaticFiles(Config);
}

[PublicAPI]
public static class StaticFilesFeatureIAppBuilder
{
    public static IAppBuilder StaticFiles(this IAppBuilder builder, Action<StaticFileOptions, IAppCreationContext> config)
        => builder.InjectFeature((x) => {
            var opt = new StaticFileOptions();
            config(opt, x);
            return new StaticFilesFeature(opt);
        });
}


[PublicAPI]
public record ResponseCachingFeature : AppFeature
{
    public override void BeforeRun(WebApplication webBuilder)
        => webBuilder.UseResponseCaching();

    public override void BeforeRegistrations(WebApplicationBuilder webBuilder) 
        => webBuilder.Services.AddResponseCaching();
}

[PublicAPI]
public static class ResponseCachingFeatureIAppBuilder
{
    public static IAppBuilder ResponseCaching(this IAppBuilder builder)
        => builder.InjectFeature(() => new ResponseCachingFeature());
}


[PublicAPI]
public record DevelopmentFeature(bool IsEnabled) : AppFeature
{
    public override void BeforeRun(WebApplication app)
    {
        if (!IsEnabled)
            return;
        app.UseExceptionHandler();
        app.UseStatusCodePages();
        app.UseDeveloperExceptionPage();
    }
    public override void BeforeRegistrations(WebApplicationBuilder webBuilder)
    {
        if (!IsEnabled)
            return;
        webBuilder.Services.AddProblemDetails();
    }
}

[PublicAPI]
public static class DevelopmentFeatureIAppBuilder
{
    public static IAppBuilder Development(this IAppBuilder builder, bool enabled)
        => builder.InjectFeature(() => new DevelopmentFeature(enabled));

    public static IAppBuilder Development(this IAppBuilder builder)
        => builder.InjectFeature(x => new DevelopmentFeature(x.Environment.IsDevelopment()));
}
