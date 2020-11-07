using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CosmosDb.Repositories.Abstractions
{
    public interface IEntity
    {
        [BsonId]
        [BsonElement("_id")]
        ObjectId Id { get; set; }
    }
}
