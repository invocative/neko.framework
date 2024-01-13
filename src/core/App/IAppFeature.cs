namespace Invocative.Neko.Framework.App;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public interface IAppFeature
{
    string FeatureName { get; }
    void BeforeRegistrations(WebApplicationBuilder webBuilder);
    void AfterRegistration(WebApplicationBuilder webBuilder);
    void BeforeRun(WebApplication webBuilder);
}

public interface IAppFeatureWithHealthCheck : IAppFeature
{
    void HealthCheckRegistration(IHealthChecksBuilder builder);
}
