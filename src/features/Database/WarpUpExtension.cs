namespace Database;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class WarpUpExtension
{
    // ReSharper disable once FlagArgument
    public static WebApplication WarpUp<T>(this WebApplication app, bool isMigrate = true) where T : DbContext
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<T>();
        if (isMigrate)
            db.Database.Migrate();
        else
            db.Database.EnsureCreated();
        return app;
    }
}
