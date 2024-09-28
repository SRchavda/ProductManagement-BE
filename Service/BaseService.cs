using CrudWithMongoDB.Model.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CrudWithMongoDB.Service
{
    public class BaseService<T> where T : class, IBaseEntity, new()
    {
        private readonly IMongoCollection<T> _collection;

        public BaseService(MongoRepository mongoRepo)
        {
            string className = typeof(T).Name;
            _collection = mongoRepo.mongoDatabase.GetCollection<T>(className);
        }

        public virtual async Task<List<T>> GetAsync()
        {
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }

        public virtual async Task<T?> GetAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<string> CreateAsync(T newItem)
        {
            newItem.Id = null;
            await _collection.InsertOneAsync(newItem);
            return newItem.Id;
        }

        public virtual async Task<T> UpdateAsync(string id, T updatedItem)
        {
            var existingItem = await GetAsync(id);
            if (existingItem == null)
            {
                throw new KeyNotFoundException($"Item with ID {id} not found.");
            }
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedItem);
            return await GetAsync(id);
        }

        public virtual async Task RemoveAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
