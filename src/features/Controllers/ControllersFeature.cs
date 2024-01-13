namespace Controllers;

using Invocative.Neko.Framework.App;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Http;

[PublicAPI]
public record ControllersFeature(Action<MvcOptions>? Configure) : AppFeature
{
    public override void BeforeRegistrations(WebApplicationBuilder webBuilder)
    {
        webBuilder.Services.AddTransient(s => 
            s.GetRequiredService<IHttpContextAccessor>().HttpContext!.User);
        webBuilder.Services.AddHttpContextAccessor();
        webBuilder.Services
            .AddControllers(this.Configure)
            .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = context =>
                new BadRequestObjectResult(context.ModelState) {
                    ContentTypes = { System.Net.Mime.MediaTypeNames.Application.Json }
                })
            .AddJson()
            .AddEndpointsApiExplorer();
    }

    public override void BeforeRun(WebApplication webBuilder) 
        => webBuilder.MapControllers();
}

[PublicAPI]
public record CorsFeature(Action<CorsOptions> Configure) : AppFeature
{
    public override void BeforeRegistrations(WebApplicationBuilder webBuilder) 
        => webBuilder.Services.AddCors(Configure);

    public override void BeforeRun(WebApplication webBuilder)
        => webBuilder.UseCors();
}

public static class JsonExtension
{
    [PublicAPI]
    public static IServiceCollection AddJson(this IMvcBuilder mvc) =>
        mvc.AddNewtonsoftJson(x => {
            x.SerializerSettings.Formatting = Formatting.Indented;
            x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            x.SerializerSettings.Converters.Add(new StringEnumConverter());
            x.SerializerSettings.Converters.Add(new UnixDateTimeConverter());
            x.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
        }).Services;
}


[PublicAPI]
public static class ControllersFeatureIAppBuilder
{
    public static IAppBuilder Controllers(this IAppBuilder builder, Action<MvcOptions>? config = null)
        => builder.InjectFeature(() => new ControllersFeature(config));
    public static IAppBuilder Cors(this IAppBuilder builder, Action<CorsOptions> config)
        => builder.InjectFeature(() => new CorsFeature(config));
}
