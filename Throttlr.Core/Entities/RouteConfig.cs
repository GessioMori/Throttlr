namespace Throttlr.Core.Entities;
public record class RouteConfig
{
    public string? Id { get; set; }
    public required string Path { get; init; }
    public required string UpstreamUrl { get; init; }
    public required string HttpVerb { get; init; }
    public HttpMethod Method => new(this.HttpVerb);
}