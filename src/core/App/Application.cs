namespace Invocative.Neko.Framework.App;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utils;

public class Application
{
    public static IAppBuilder CreateBuilder(params string[] args) => new AppBuilder(args);
}

public interface IAppCreationContext
{
    IAppServiceBuilder AppService { get; }
    WebApplicationBuilder AspBuilder { get; }
}

internal class AppBuilder : IAppBuilder, IAppCreationContext
{
    private readonly WebApplicationBuilder _aspBuilder;
    private readonly AppServiceBuilder _appServiceBuilder;
    private readonly IServiceCollection _builderServices;

    private List<IAppFeature> Features { get; } = new();

    public AppBuilder(string[] args)
    {
        _aspBuilder = WebApplication.CreateBuilder(args);
        _appServiceBuilder = new AppServiceBuilder(_aspBuilder);
        _builderServices = new ServiceCollection();
    }

    public IAppBuilder Configuration(Action<IConfigurationBuilder> config)
    {
        config(_aspBuilder.Configuration);
        return this;
    }

    public IAppBuilder Services(Action<IAppServiceBuilder> config)
    {
        config(_appServiceBuilder);
        return this;
    }

    public IAppBuilder InjectFeature<T>(Func<T> ctor) where T : IAppFeature
    {
        Features.Add(ctor());
        return this;
    }

    public IAppBuilder InjectFeature<T>(Func<IAppCreationContext, T> ctor) where T : IAppFeature
    {
        Features.Add(ctor(this));
        return this;
    }

    public IApp Build()
    {
        _aspBuilder.Configuration
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables();
        Features.ForEach(x => x.BeforeRegistrations(_aspBuilder));
        var hl = _aspBuilder.Services.AddHealthChecks();
        Features.OfType<IAppFeatureWithHealthCheck>().ForEach(x => x.HealthCheckRegistration(hl));
        Features.ForEach(x => x.AfterRegistration(_aspBuilder));
        var app = _aspBuilder.Build();
        return new AppInstance(Features, app);
    }

    IAppServiceBuilder IAppCreationContext.AppService => this._appServiceBuilder;
    WebApplicationBuilder IAppCreationContext.AspBuilder => this._aspBuilder;
}

internal class AppInstance(List<IAppFeature> features, WebApplication app) : IApp
{
    public IApp On(Action<IApplicationBuilder> config)
    {
        config(app);
        return this;
    }

    public async ValueTask RunAsync()
    {
        app.UseHealthChecks("/health");
        features.ForEach(x => x.BeforeRun(app));
        await app.RunAsync();
    }
}

public abstract record AppFeature : IAppFeature
{
    public string FeatureName => this.GetType().Name;

    public virtual void BeforeRegistrations(WebApplicationBuilder webBuilder)
    {
    }
    public virtual void AfterRegistration(WebApplicationBuilder webBuilder)
    {
    }
    public virtual void BeforeRun(WebApplication webBuilder)
    {
    }
}
