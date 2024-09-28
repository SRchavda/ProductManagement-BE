using CrudWithMongoDB.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CrudWithMongoDB.Service
{
    public class MongoRepository
    {
        public IMongoDatabase mongoDatabase;
        public MongoRepository(IOptions<MongoDBSettings> mongodbSettings)
        {
            MongoClient mongoClient = new(mongodbSettings.Value.ConnectionString);
            mongoDatabase = mongoClient.GetDatabase(mongodbSettings.Value.DatabaseName);
        }
    }   
}
