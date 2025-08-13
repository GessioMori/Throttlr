using Throttlr.Core.Entities;
using Throttlr.Core.Interfaces;

namespace Throttlr.Application.Services;
public class ReverseProxyService : IReverseProxyService
{
    private readonly IRouteService _routeService;
    private readonly IRequestForwarder _requestForwarder;

    public ReverseProxyService(IRouteService routeService, IRequestForwarder requestForwarder)
    {
        this._routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
        this._requestForwarder = requestForwarder ?? throw new ArgumentNullException(nameof(requestForwarder));
    }

    public async Task<HttpResponseMessage?> HandleRequestAsync(ProxyRequest proxyRequest)
    {
        RouteConfig? routeConfig = this._routeService.GetRoute(proxyRequest.Path, proxyRequest.Method);

        if (routeConfig is null)
        {
            return null;
        }

        return await this._requestForwarder.ForwardAsync(proxyRequest, routeConfig);
    }
}