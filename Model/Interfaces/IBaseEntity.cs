using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrudWithMongoDB.Model.Interfaces
{
    public interface IBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string? Id { get; set; }
    }
}
