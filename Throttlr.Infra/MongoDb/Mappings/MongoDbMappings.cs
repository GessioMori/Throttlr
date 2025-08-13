using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Throttlr.Core.Entities;

namespace Throttlr.Infra.MongoDb.Mappings;
public static class MongoDbMappings
{
    public static void RegisterMappings()
    {
        RouteConfigRegisterMappings();
    }

    public static void RouteConfigRegisterMappings()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(RouteConfig))) return;

        BsonClassMap.RegisterClassMap<RouteConfig>(cm =>
        {
            cm.AutoMap();

            cm.MapIdMember(c => c.Id)
            .SetSerializer(new StringSerializer(BsonType.ObjectId))
            .SetElementName("_id")
            .SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.StringObjectIdGenerator.Instance);

            cm.MapMember(c => c.Path).SetElementName("path")
                .SetIsRequired(true);

            cm.MapMember(c => c.UpstreamUrl).SetElementName("upstreamUrl")
                .SetIsRequired(true);

            cm.MapMember(c => c.HttpVerb).SetElementName("httpVerb")
                .SetIsRequired(true);
        });
    }
}