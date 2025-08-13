using Throttlr.Core.Entities;

namespace Throttlr.Core.Interfaces;
public interface IRequestForwarder
{
    Task<HttpResponseMessage> ForwardAsync(ProxyRequest context, RouteConfig routeConfig);
}