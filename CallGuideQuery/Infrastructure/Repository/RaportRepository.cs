using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entity;
using MongoDB.Driver;

namespace Infrastructure.Repository
{
    public class RaportRepository : IRaportRepository
    {
        private readonly IMongoCollection<Raport> _collection;

        public RaportRepository(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Raport>(typeof(Raport).Name.ToLowerInvariant());
        }
        public virtual Task<bool> CreateAsync(Raport model)
        {
            _collection.InsertOneAsync(model);
            return Task.FromResult(true);
        }

        public virtual async Task<Raport> GetRaport(int userId)
        {
            var result = await _collection.Find(r=>r.UserId==userId).FirstOrDefaultAsync();
            return result;
        }
    }
}