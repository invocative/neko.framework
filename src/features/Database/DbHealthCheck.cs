namespace Database;

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

[PublicAPI]
public class DbHealthCheck(IServiceProvider serviceProvider, ILogger<DbHealthCheck> logger)
    : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        try
        {
            var ctx = scope.ServiceProvider.GetRequiredService<DbContext>();
            await ctx.Database.OpenConnectionAsync(ct);
            await ctx.Database.CloseConnectionAsync();
        }
        catch (Exception e)
        {
            logger.LogTrace(e, "failed connect to db");
            return HealthCheckResult.Degraded("Failed db connect", e);
        }
        return HealthCheckResult.Healthy();
    }
}
