using Throttlr.Core.Entities;

namespace Throttlr.Infra.Interfaces;
public interface IRouteConfigRepository
{
    Task<IEnumerable<RouteConfig>> GetAllAsync();
    Task<RouteConfig> GetByPathAsync(string path);
}