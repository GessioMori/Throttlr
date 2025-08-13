using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Throttlr.Core.Entities;
public record class RouteConfig
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("path")]
    public required string Path { get; init; }

    [BsonElement("upstreamUrl")]
    public required string UpstreamUrl { get; init; }

    [BsonElement("httpVerb")]
    public required string HttpVerb { get; init; }
    public HttpMethod Method => new(this.HttpVerb);
}