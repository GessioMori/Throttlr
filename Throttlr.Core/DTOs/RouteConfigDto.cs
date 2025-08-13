namespace Throttlr.Core.DTOs;
public record class RouteConfigDto
{
    public required string Path { get; init; }
    public required string UpstreamUrl { get; init; }
    public required string HttpVerb { get; init; }
}