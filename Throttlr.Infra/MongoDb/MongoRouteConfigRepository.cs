using MongoDB.Driver;
using Throttlr.Core.Entities;
using Throttlr.Infra.Interfaces;

namespace Throttlr.Infra.MongoDb;
public class MongoRouteConfigRepository : IRouteConfigRepository
{
    private readonly IMongoCollection<RouteConfig> _collection;

    public MongoRouteConfigRepository(IMongoDatabase database)
    {
        this._collection = database.GetCollection<RouteConfig>(MongoCollections.Routes);
    }

    public async Task<IEnumerable<RouteConfig>> GetAllAsync()
    {
        return await this._collection.Find(FilterDefinition<RouteConfig>.Empty).ToListAsync();
    }

    public async Task<RouteConfig> GetByPathAsync(string path)
    {
        return await this._collection.Find(x => x.Path == path).FirstOrDefaultAsync();
    }
}