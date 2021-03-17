using Domain.Interfaces;
using Domain.Entity;
using MongoDB.Driver;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infrastructure.Repository
{
    public class CallGuideMongoDbRepository<T> : ICallGuideMongoDbRepository<T> where T :BaseEntity
    {
        private readonly IMongoCollection<T> _collection;
        public CallGuideMongoDbRepository(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }
        public virtual IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _collection.Find(predicate).ToEnumerable();
        }
        public virtual async Task<T> FindById(int id)  
        {
            return await _collection
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
        public virtual async Task<bool> CreateManyAsync(IEnumerable<T> models)
        {
            await _collection.InsertManyAsync(models);
            return true;
        }

        public virtual async Task<bool> CreateAsync(T model)
        {
            await _collection.InsertOneAsync(model);
            return true;
        }

        public virtual async Task<bool> UpdateAsync(T model)
        {
            var updateResult =
                await _collection
                    .ReplaceOneAsync(filter: f => f.Id == model.Id, replacement: model);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _collection
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }
    }
}