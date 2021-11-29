using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WWA.Grains.Entities
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
    }
}
