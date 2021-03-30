using Domain.Entity;
using MongoDB.Driver;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using Application.Interfaces;
using Application.Resposes;
using Newtonsoft.Json;

namespace Infrastructure.Repository
{
    public class CallGuideMongoDbRepository : IMongoRepository
    {
        private readonly IMongoCollection<CallGuide> _collection;
        public CallGuideMongoDbRepository(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<CallGuide>(typeof(CallGuide).Name.ToLowerInvariant());
        }
        public virtual IEnumerable<CallGuide> Where(Expression<Func<CallGuide, bool>> predicate)
        {
            //async yap
            return _collection.Find(predicate).ToEnumerable();
        }
        public virtual Task<CallGuide> FindById(int id)
        {
            return _collection
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
        public virtual async Task<bool> CreateManyAsync(IEnumerable<CallGuide> models)
        {
            await _collection.InsertManyAsync(models);
            return true;
        }

        public virtual async Task<bool> CreateAsync(CallGuide model)
        {
            await _collection.InsertOneAsync(model);
            return true;
        }

        public virtual async Task<bool> UpdateAsync(CallGuide model)
        {
            var updateResult =
                await _collection
                    .ReplaceOneAsync(filter: f => f.Id == model.Id, replacement: model);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            FilterDefinition<CallGuide> filter = Builders<CallGuide>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _collection
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }

        public List<RaportItem> GetRaportItem(int userId)
        {
            // var unvind = new BsonDocument{
            //             {"$unwind",
            //                 new BsonDocument{
            //                     {"path","$CommunicationInfos"},
            //                     {"preserveNullAndEmptyArrays",true}  
            //             }}
            //         };
            // var pipeline = new[] { unvind };
            // var res = _collection.Aggregate<BsonDocument>(pipeline).ToList();
            // var match = new BsonDocument
            // {
            //     {"$match",new BsonDocument{{"$UserId","11"}}}
            // };
            // var group = new BsonDocument
            // {
            //     {
            //         "$group",
            //             new BsonDocument
            //             {
            //                 {"_id",new BsonDocument{{"Location","$Location"}}},
            //                 {"Count",new BsonDocument{{"$sum","$Count"}}}
            //             }
            //     }
            // };
            // var unwindOption = new AggregateUnwindOptions<CallGuide>() { PreserveNullAndEmptyArrays = true };
            var result2 = _collection.Aggregate()
                .Match(m=>m.UserId==userId)
                .Unwind("CommunicationInfos")
                .Group(
                    new BsonDocument{{"_id","$CommunicationInfos.Location"},{"Count",new BsonDocument{{"$sum",1}}}}
                )
                .ToList();
            List<RaportItem> raportResponseDtos = new List<RaportItem>();
            
            result2.ForEach(r=>{
                raportResponseDtos.Add(new RaportItem{Location=r["_id"].ToString(),Count=r["Count"].ToInt32()});
            });
            return raportResponseDtos;
        }
    }
}