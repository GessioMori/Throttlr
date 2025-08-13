using Throttlr.Core.Entities;
using Throttlr.Core.Interfaces;
using Throttlr.Infra.Interfaces;
using Throttlr.Shared.OperationResult;

namespace Throttlr.Application.Services;
public class ReverseProxyService : IReverseProxyService
{
    private readonly IRouteConfigRepository _routeService;
    private readonly IRequestForwarder _requestForwarder;

    public ReverseProxyService(IRouteConfigRepository routeService, IRequestForwarder requestForwarder)
    {
        this._routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
        this._requestForwarder = requestForwarder ?? throw new ArgumentNullException(nameof(requestForwarder));
    }

    public async Task<HttpResponseMessage?> HandleRequestAsync(ProxyRequest proxyRequest)
    {
        OperationResult<RouteConfig> result = await this._routeService.GetByPathAndMethodAsync(proxyRequest.Path, proxyRequest.Method);

        if (!result.Success || result.Data is null)
        {
            return null;
        }

        return await this._requestForwarder.ForwardAsync(proxyRequest, result.Data);
    }
}