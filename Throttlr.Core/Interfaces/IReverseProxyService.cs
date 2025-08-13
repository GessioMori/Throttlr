using Throttlr.Core.Entities;

namespace Throttlr.Core.Interfaces;
public interface IReverseProxyService
{
    Task<HttpResponseMessage?> HandleRequestAsync(ProxyRequest proxyRequest);
}