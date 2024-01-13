namespace Invocative.Neko.Framework.App;

using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

[PublicAPI]
public interface IAppBuilder
{
    IAppBuilder Configuration(Action<IConfigurationBuilder> config);
    IAppBuilder Services(Action<IAppServiceBuilder> config);
    IAppBuilder InjectFeature<T>(Func<T> ctor) where T : IAppFeature;
    IAppBuilder InjectFeature<T>(Func<IAppCreationContext, T> ctor) where T : IAppFeature;
    IApp Build();
}
