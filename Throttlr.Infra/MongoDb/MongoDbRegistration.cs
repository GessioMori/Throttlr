using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Throttlr.Infra.MongoDb.Mappings;

namespace Throttlr.Infra.MongoDb;

public static class MongoDbRegistration
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        MongoDbSettings settings = configuration.GetSection("MongoDb").Get<MongoDbSettings>() ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddSingleton(settings);

        services.AddSingleton<IMongoClient>(sp => new MongoClient(settings.ConnectionString));

        services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(settings.DatabaseName));

        MongoDbMappings.RegisterMappings();

        return services;
    }
}