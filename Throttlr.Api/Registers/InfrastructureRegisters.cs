using Throttlr.Infra.Interfaces;
using Throttlr.Infra.MongoDb;

namespace Throttlr.Api.Registers;

public static class InfrastructureRegisters
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddMongoDb(config);

        services.AddScoped<IRouteConfigRepository, MongoRouteConfigRepository>();

        return services;
    }
}