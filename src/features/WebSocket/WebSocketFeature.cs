namespace WebSocket;

using Invocative.Neko.Framework.App;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.WebSockets;

[PublicAPI]
public record WebSocketFeature(Action<WebSocketOptions> WebsocketOptions) : AppFeature
{
    public override void BeforeRegistrations(WebApplicationBuilder webBuilder) =>
        webBuilder.Services
            .AddWebSockets(WebsocketOptions);
}


[PublicAPI]
public static class WebSocketFeatureIAppBuilder
{
    public static IAppBuilder WebSockets(this IAppBuilder builder, Action<WebSocketOptions>? config = null)
        => builder.InjectFeature(() => new WebSocketFeature(config ?? (options => { })));
}
