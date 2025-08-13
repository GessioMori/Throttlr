using Throttlr.Core.Entities;
using Throttlr.Core.Interfaces;

namespace Throttlr.Application.Forwarders;

public class HttpClientRequestForwarder : IRequestForwarder
{
    private readonly HttpClient _httpClient;

    public HttpClientRequestForwarder(HttpClient client)
    {
        this._httpClient = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<HttpResponseMessage> ForwardAsync(ProxyRequest context, RouteConfig routeConfig)
    {
        UriBuilder uriBuilder = new(routeConfig.UpstreamUrl)
        {
            Path = routeConfig.Path,
            Query = context.QueryString
        };

        string? content = context.Body is not null ? await context.Body.ReadToEndAsync() : null;

        HttpRequestMessage requestMessage = new(context.Method, uriBuilder.ToString())
        {
            Content = content is not null ? new StringContent(content) : null
        };

        foreach (KeyValuePair<string, string?[]> header in context.Headers)
        {
            if (header.Value is not null)
            {
                foreach (string? value in header.Value)
                {
                    if (value is null || header.Key.Equals("Host"))
                    {
                        continue;
                    }
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, value);
                }
            }
        }

        return await this._httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
    }
}