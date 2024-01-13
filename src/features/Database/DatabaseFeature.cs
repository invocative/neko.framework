namespace Database;

using Invocative.Neko.Framework.App;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public enum DatabaseSetupKind
{
    Classic,
    Factory,
    Pool,
    FactoryPool
}

[PublicAPI]
public record DatabaseFeature<T>(Action<IDatabaseAdapter> Setup, IAppCreationContext BuildContext, DatabaseSetupKind Kind) : AppFeature, IAppFeatureWithHealthCheck where T : DbContext
{
    public override void BeforeRegistrations(WebApplicationBuilder webBuilder)
    {
        _ = this.Kind switch {
            DatabaseSetupKind.Classic => webBuilder.Services.AddDbContext<T>(x =>
                this.Setup(new DatabaseAdapter(x, this.BuildContext))),
            DatabaseSetupKind.Factory => webBuilder.Services.AddDbContextFactory<T>(x =>
                this.Setup(new DatabaseAdapter(x, this.BuildContext))),
            DatabaseSetupKind.Pool => webBuilder.Services.AddDbContextPool<T>(x =>
                this.Setup(new DatabaseAdapter(x, this.BuildContext))),
            DatabaseSetupKind.FactoryPool => webBuilder.Services.AddPooledDbContextFactory<T>(x =>
                this.Setup(new DatabaseAdapter(x, this.BuildContext)))
        };
    }

    public override void BeforeRun(WebApplication webBuilder)
        => webBuilder.WarmUp<T>();

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
