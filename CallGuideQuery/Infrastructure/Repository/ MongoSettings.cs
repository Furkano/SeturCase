using Domain.Interfaces;

namespace Infrastructure.Repository
{
    public class  MongoSettings : IMongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}