using MongoDB.Driver;
using Throttlr.Core.Entities;
using Throttlr.Infra.Interfaces;
using Throttlr.Shared.OperationResult;

namespace Throttlr.Infra.MongoDb;
public class MongoRouteConfigRepository : IRouteConfigRepository
{
    private readonly IMongoCollection<RouteConfig> _collection;

    public MongoRouteConfigRepository(IMongoDatabase database)
    {
        this._collection = database.GetCollection<RouteConfig>(MongoCollections.Routes);
    }

    public async Task<OperationResult<string>> CreateAsync(RouteConfig route)
    {
        try
        {
            await this._collection.InsertOneAsync(route);
            return OperationResult<string>.Ok(route.Id ?? string.Empty);
        }
        catch (Exception ex)
        {
            return OperationResult<string>.Fail($"Error creating route: {ex.Message}");
        }
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        DeleteResult result = await this._collection.DeleteOneAsync(r => r.Id == id);

        if (result.DeletedCount == 1) return OperationResult.Ok();
        if (result.DeletedCount == 0) return OperationResult.Fail("No document found with the specified ID.");
        return OperationResult.Fail("Multiple documents deleted, which should not happen.");
    }

    public async Task<OperationResult<RouteConfig>> GetByIdAsync(string id)
    {
        RouteConfig result = await this._collection.Find(r => r.Id == id).FirstOrDefaultAsync();

        if (result != null)
        {
            return OperationResult<RouteConfig>.Ok(result);
        }
        else
        {
            return OperationResult<RouteConfig>.Fail("No route found with the specified ID.");
        }
    }

    public async Task<OperationResult<RouteConfig>> GetByPathAndMethodAsync(string path, HttpMethod httpMethod)
    {
        RouteConfig result = await this._collection.Find(r => r.Path == path && r.HttpVerb == httpMethod.ToString())
            .FirstOrDefaultAsync();

        if (result != null)
        {
            return OperationResult<RouteConfig>.Ok(result);
        }
        else
        {
            return OperationResult<RouteConfig>.Fail("No route found for the specified path and method.");
        }
    }

    public async Task<OperationResult<RouteConfig>> UpdateAsync(string id, RouteConfig route)
    {
        ReplaceOneResult result = await this._collection.ReplaceOneAsync(r => r.Id == id, route);

        if (result.ModifiedCount == 1) return OperationResult<RouteConfig>.Ok(route);
        if (result.ModifiedCount == 0) return OperationResult<RouteConfig>.Fail("No document found with the specified ID.");
        return OperationResult<RouteConfig>.Fail("Multiple documents updated, which should not happen.");
    }
}