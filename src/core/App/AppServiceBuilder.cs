namespace Invocative.Neko.Framework.App;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

internal class AppServiceBuilder(WebApplicationBuilder appBuilder) : IAppServiceBuilder
{
    public IAppServiceBuilder Singleton<TInterface, TImpl>() where TInterface : class where TImpl : class, TInterface
    {
        appBuilder.Services.AddSingleton<TInterface, TImpl>();
        return this;
    }

    public IAppServiceBuilder Singleton<TImpl>() where TImpl : class
    {
        appBuilder.Services.AddSingleton<TImpl>();
        return this;
    }

    public IAppServiceBuilder Scoped<TInterface, TImpl>() where TInterface : class where TImpl : class, TInterface
    {
        appBuilder.Services.AddScoped<TInterface, TImpl>();
        return this;
    }

    public IAppServiceBuilder Scoped<TImpl>() where TImpl : class
    {
        appBuilder.Services.AddScoped<TImpl>();
        return this;
    }

    public IAppServiceBuilder Transient<TInterface, TImpl>() where TInterface : class where TImpl : class, TInterface
    {
        appBuilder.Services.AddTransient<TInterface, TImpl>();
        return this;
    }

    public IAppServiceBuilder Transient<TImpl>() where TImpl : class
    {
        appBuilder.Services.AddTransient<TImpl>();
        return this;
    }
#if NET8_0_OR_GREATER
    public IAppServiceBuilder KeyedSingleton<TInterface, TImpl>(object key) where TInterface : class where TImpl : class, TInterface
    {
        appBuilder.Services.AddKeyedSingleton<TInterface, TImpl>(key);
        return this;
    }

    public IAppServiceBuilder KeyedSingleton<TImpl>(object key) where TImpl : class
    {
        appBuilder.Services.AddKeyedSingleton<TImpl>(key);
        return this;
    }

    public IAppServiceBuilder KeyedScoped<TInterface, TImpl>(object key) where TInterface : class where TImpl : class, TInterface
    {
        appBuilder.Services.AddKeyedScoped<TInterface, TImpl>(key);
        return this;
    }

    public IAppServiceBuilder KeyedScoped<TImpl>(object key) where TImpl : class
    {
        appBuilder.Services.AddKeyedScoped<TImpl>(key);
        return this;
    }

    public IAppServiceBuilder KeyedTransient<TInterface, TImpl>(object key) where TInterface : class where TImpl : class, TInterface
    {
        appBuilder.Services.AddKeyedTransient<TInterface, TImpl>(key);
        return this;
    }

    public IAppServiceBuilder KeyedTransient<TImpl>(object key) where TImpl : class
    {
        appBuilder.Services.AddKeyedTransient<TImpl>(key);
        return this;
    }
#endif
}
