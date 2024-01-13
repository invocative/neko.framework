namespace Invocative.Neko.Feature.Stl.Fusion;

using Framework.App;
using global::Stl.Fusion;
using global::Stl.Rpc.Server;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

[PublicAPI]
public record FusionFeature(Action<FusionBuilder> Builder) : AppFeature
{
    public override void BeforeRegistrations(WebApplicationBuilder webBuilder) =>
        Builder(webBuilder.Services
            .AddFusion());

    public override void BeforeRun(WebApplication webBuilder) 
        => webBuilder.MapRpcWebSocketServer();
}


[PublicAPI]
public static class FusionFeatureIAppBuilder
{
    public static IAppBuilder Fusion(this IAppBuilder builder, Action<FusionBuilder> fusionBuilder)
        => builder.InjectFeature(() => new FusionFeature(fusionBuilder));
}
