namespace Throttlr.Core.DTOs;
public record class RouteConfigReadDto
{
    public string Id { get; init; } = default!;
    public string Path { get; init; } = default!;
    public string UpstreamUrl { get; init; } = default!;
    public string HttpVerb { get; init; } = default!;
}