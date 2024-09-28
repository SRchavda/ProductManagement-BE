using CrudWithMongoDB.Model.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CrudWithMongoDB.Model
{
    public class BaseEntity : IBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
