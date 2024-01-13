namespace Database;

using Invocative.Neko.Framework.App;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

[PublicAPI]
public record DatabaseFeature<T>(Action<IDatabaseAdapter> Setup, IAppCreationContext BuildContext) : AppFeature, IAppFeatureWithHealthCheck where T : DbContext
{
    public override void BeforeRegistrations(WebApplicationBuilder webBuilder)
        => webBuilder.Services.AddDbContext<T>(x => this.Setup(new DatabaseAdapter(x, BuildContext)));

    public override void BeforeRun(WebApplication webBuilder)
        => webBuilder.WarpUp<T>();

    public void HealthCheckRegistration(IHealthChecksBuilder builder)
        => builder.AddCheck<DbHealthCheck>("db", HealthStatus.Degraded, new[] { "database" });
}

public class DatabaseAdapter(DbContextOptionsBuilder builder, IAppCreationContext builderContext) : IDatabaseAdapter, IDatabaseDbContextOptionsAccessor
{
    public DbContextOptionsBuilder Builder { get; } = builder;
    public IAppCreationContext BuildContext { get; } = builderContext;

    public IDatabaseAdapter UseLazyLoading()
    {
        Builder.UseLazyLoadingProxies();
        return this;
    }

    // ReSharper disable once FlagArgument
    public IDatabaseAdapter UseDiagnostic(bool sensitiveEnabled = false)
    {
        if(sensitiveEnabled)
            Builder.EnableSensitiveDataLogging();
        Builder.EnableDetailedErrors();
        return this;
    }
}

public interface IDatabaseDbContextOptionsAccessor
{
    DbContextOptionsBuilder Builder { get; }
    IAppCreationContext BuildContext { get; }
}

public interface IDatabaseAdapter
{
    IDatabaseAdapter UseLazyLoading();
    IDatabaseAdapter UseDiagnostic(bool sensitiveEnabled = false);
}
