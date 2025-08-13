using Throttlr.Core.Entities;
using Throttlr.Core.Interfaces;

namespace Throttlr.Api;

public class ReverseProxyMiddleware
{
    private readonly RequestDelegate _next;

    public ReverseProxyMiddleware(RequestDelegate next)
    {
        this._next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context, IReverseProxyService reverseProxyService)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Request.Path.StartsWithSegments("/api"))
        {
            await this._next(context);
            return;
        }

        ArgumentNullException.ThrowIfNull(reverseProxyService);

        ProxyRequest proxyRequest = new(context.Request.Path,
            new HttpMethod(context.Request.Method),
            context.Request.BodyReader is not null ? new StreamReader(context.Request.Body) : null,
            context.Request.QueryString.Value ?? string.Empty,
            context.Request.Headers.ToDictionary(
                header => header.Key,
                header => header.Value.ToArray()
            ));

        HttpResponseMessage? responseMessage = await reverseProxyService.HandleRequestAsync(proxyRequest);

        if (responseMessage is null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("No matching route found.");
            return;
        }
        context.Response.StatusCode = (int)responseMessage.StatusCode;
        foreach (KeyValuePair<string, IEnumerable<string>> header in responseMessage.Headers)
        {
            context.Response.Headers[header.Key] = header.Value.ToArray();
        }

        if (responseMessage.Content is not null)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            using Stream responseStream = await responseMessage.Content.ReadAsStreamAsync();
            await responseStream.CopyToAsync(context.Response.Body);
        }
    }
}