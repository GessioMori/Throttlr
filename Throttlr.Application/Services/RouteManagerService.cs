using Throttlr.Core.Entities;
using Throttlr.Core.Interfaces;
using Throttlr.Infra.Interfaces;
using Throttlr.Shared.OperationResult;

namespace Throttlr.Application.Services;
public class RouteManagerService : IRouteManagerService
{
    private readonly IRouteConfigRepository routeConfigRepository;

    public RouteManagerService(IRouteConfigRepository routeConfigRepository)
    {
        this.routeConfigRepository = routeConfigRepository;
    }

    public async Task<OperationResult<string>> CreateAsync(RouteConfig route)
    {
        OperationResult<string> result = await this.routeConfigRepository.CreateAsync(route);

        if (!result.Success || result.Data is null)
        {
            return OperationResult<string>.Fail("Failed to create the route.");
        }

        return OperationResult<string>.Ok(result.Data);
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        OperationResult result = await this.routeConfigRepository.DeleteAsync(id);

        if (!result.Success)
        {
            return OperationResult.Fail("No route found for the specified ID.");
        }
        return OperationResult.Ok();
    }

    public async Task<OperationResult<RouteConfig>> GetByIdAsync(string id)
    {
        OperationResult<RouteConfig> result = await this.routeConfigRepository.GetByIdAsync(id);

        if (!result.Success || result.Data is null)
        {
            return OperationResult<RouteConfig>.Fail("No route found for the specified ID.");
        }

        return OperationResult<RouteConfig>.Ok(result.Data);
    }

    public async Task<OperationResult<RouteConfig>> GetByPathAndMethodAsync(string path, HttpMethod httpMethod)
    {
        OperationResult<RouteConfig> route = await this.routeConfigRepository.GetByPathAndMethodAsync(path, httpMethod);

        if (!route.Success || route.Data is null)
        {
            return OperationResult<RouteConfig>.Fail("No route found for the specified path and method.");
        }

        return OperationResult<RouteConfig>.Ok(route.Data);
    }

    public async Task<OperationResult> UpdateAsync(string id, RouteConfig route)
    {
        OperationResult<RouteConfig> result = await this.routeConfigRepository.UpdateAsync(id, route);

        if (!result.Success || result.Data is null)
        {
            return OperationResult.Fail("No route found for the specified ID.");
        }

        return OperationResult.Ok();
    }
}