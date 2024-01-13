namespace Invocative.Neko.Framework.App;

using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

[PublicAPI]
public interface IApp
{
    IApp On(Action<IApplicationBuilder> config);
    ValueTask RunAsync();
}
