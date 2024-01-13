

using Controllers;
using Database;
using Database.Npgsql;
using Invocative.Neko.Feature.Stl.Fusion;
using Invocative.Neko.Framework.App;
using Microsoft.EntityFrameworkCore;
using WebSocket;

Application.CreateBuilder(args)
    .Database<FooBar>(x => x
        .UseDiagnostic()
        .UseLazyLoading()
        .UseNpgsql())
    .WebSockets()
    .Cors(x => { });



public class FooBar : DbContext
{

}
