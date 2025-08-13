using Throttlr.Core.Entities;
using Throttlr.Shared.OperationResult;

namespace Throttlr.Core.Interfaces;
public interface IRouteManagerService
{
    Task<OperationResult<RouteConfig>> GetByPathAndMethodAsync(string path, HttpMethod httpMethod);
    Task<OperationResult<RouteConfig>> GetByIdAsync(string id);
    Task<OperationResult<string>> CreateAsync(RouteConfig route);
    Task<OperationResult> UpdateAsync(string id, RouteConfig route);
    Task<OperationResult> DeleteAsync(string id);
}