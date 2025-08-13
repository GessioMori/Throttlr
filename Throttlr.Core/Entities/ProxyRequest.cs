namespace Throttlr.Core.Entities;

public record ProxyRequest
{
    public string Path { get; init; }
    public HttpMethod Method { get; init; }
    public string? QueryString { get; init; }
    public StreamReader? Body { get; init; }
    public Dictionary<string, string?[]> Headers { get; init; } = [];
    public ProxyRequest(
        string path,
        HttpMethod method,
        StreamReader? body,
        string? queryString = null,
        Dictionary<string, string?[]>? headers = null)
    {
        this.Path = path;
        this.Method = method;
        this.QueryString = queryString;
        this.Body = body;
        this.Headers = headers ?? [];
    }
}