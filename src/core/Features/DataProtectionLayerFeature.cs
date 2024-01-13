namespace Invocative.Neko.Framework.Features;

using App;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

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
    public static IAppBuilder StaticFiles(this IAppBuilder builder, Action<StaticFileOptions> config)
        => builder.InjectFeature(() => {
            var opt = new StaticFileOptions();
            config(opt);
            return new StaticFilesFeature(opt);
        });
}
