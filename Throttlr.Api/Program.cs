using FluentValidation;
using Throttlr.Api.Registers;
using Throttlr.Api.Validators;
using Throttlr.Application.Forwarders;
using Throttlr.Application.Services;
using Throttlr.Core.DTOs;
using Throttlr.Core.Interfaces;
using Throttlr.Core.Mappers;

namespace Throttlr.Api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddOpenApi();

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddSingleton<IEnvironmentService, AspNetCoreEnvironmentService>();
        builder.Services.AddHttpClient<IRequestForwarder, HttpClientRequestForwarder>();
        builder.Services.AddScoped<IRouteManagerService, RouteManagerService>();
        builder.Services.AddScoped<IReverseProxyService, ReverseProxyService>();

        builder.Services.AddMvc();
        builder.Services.AddSingleton<IValidator<RouteConfigDto>, RouteConfigDtoValidator>();
        builder.Services.AddKeyedSingleton<IValidator<string>, ObjectIdValidator>("IdValidator");
        builder.Services.AddScoped<IValidatorService, ValidatorService>();

        builder.Services.AddAutoMapper(cfg => { }, typeof(RouteConfigProfile));

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Throttlr API V1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseMiddleware<ReverseProxyMiddleware>();
        app.MapControllers();
        app.Run();
    }
}