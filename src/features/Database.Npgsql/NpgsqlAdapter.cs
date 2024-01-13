namespace Database.Npgsql;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public static class NpgsqlAdapterEx
{
    public static IDatabaseAdapter UseNpgsql(this IDatabaseAdapter adapter)
    {
        if (adapter is not IDatabaseDbContextOptionsAccessor accessor)
            throw new InvalidOperationException($"DatabaseAdapter type is implement IDatabaseDbContextOptionsAccessor");
        accessor.Builder.UseNpgsql(accessor.BuildContext.AspBuilder.Configuration.GetConnectionString("postgres"));
        return adapter;
    }
}
