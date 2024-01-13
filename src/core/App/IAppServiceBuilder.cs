namespace Invocative.Neko.Framework.App;

using JetBrains.Annotations;

#if NET8_0_OR_GREATER
[PublicAPI]
public interface IAppServiceBuilderWithKeyed
{
    IAppServiceBuilder KeyedSingleton<TInterface, TImpl>(object key) where TInterface : class where TImpl : class, TInterface;
    IAppServiceBuilder KeyedSingleton<TImpl>(object key) where TImpl : class;

    IAppServiceBuilder KeyedScoped<TInterface, TImpl>(object key) where TInterface : class where TImpl : class, TInterface;
    IAppServiceBuilder KeyedScoped<TImpl>(object key) where TImpl : class;

    IAppServiceBuilder KeyedTransient<TInterface, TImpl>(object key) where TInterface : class where TImpl : class, TInterface;
    IAppServiceBuilder KeyedTransient<TImpl>(object key) where TImpl : class;
}
#endif


[PublicAPI]
public interface IAppServiceBuilder

#if NET8_0_OR_GREATER
    : IAppServiceBuilderWithKeyed
#endif

{
    IAppServiceBuilder Singleton<TInterface, TImpl>() where TInterface : class where TImpl : class, TInterface;
    IAppServiceBuilder Singleton<TImpl>() where TImpl : class;

    IAppServiceBuilder Scoped<TInterface, TImpl>() where TInterface : class where TImpl : class, TInterface;
    IAppServiceBuilder Scoped<TImpl>() where TImpl : class;

    IAppServiceBuilder Transient<TInterface, TImpl>() where TInterface : class where TImpl : class, TInterface;
    IAppServiceBuilder Transient<TImpl>() where TImpl : class;
}
