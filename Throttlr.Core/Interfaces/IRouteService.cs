using Throttlr.Core.Entities;

namespace Throttlr.Core.Interfaces;
public interface IRouteService
{
    RouteConfig? GetRoute(string path, HttpMethod method);
}