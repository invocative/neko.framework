namespace Database;

using Invocative.Neko.Framework.App;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

[PublicAPI]
public static class DatabaseFeatureIAppBuilder
{
    public static IAppBuilder Database<T>(this IAppBuilder builder, Action<IDatabaseAdapter> setup) where T : DbContext
        => builder.InjectFeature((x) => new DatabaseFeature<T>(setup, x));
}
