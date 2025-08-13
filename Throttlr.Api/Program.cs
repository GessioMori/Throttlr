using Throttlr.Api.Registers;
using Throttlr.Application.Forwarders;
using Throttlr.Application.Services;
using Throttlr.Core.Interfaces;
using Throttlr.Infra.Interfaces;
using Throttlr.Infra.Json;
using Throttlr.Infra.MongoDb;

namespace Throttlr.Api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddSingleton<IEnvironmentService, AspNetCoreEnvironmentService>();
        builder.Services.AddHttpClient<IRequestForwarder, HttpClientRequestForwarder>();
        builder.Services.AddScoped<IReverseProxyService, ReverseProxyService>();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseMiddleware<ReverseProxyMiddleware>();

        app.MapControllers();
        app.Run();
    }
}