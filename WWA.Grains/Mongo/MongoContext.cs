using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WWA.Configuration;

namespace WWA.Grains.Mongo
{
    public class MongoContext : IMongoContext
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        public MongoContext(IOptions<MongoConfiguration> mongoConfig)
        {
            var urlBuilder = new MongoUrlBuilder(mongoConfig.Value.ConnectionString);
            Client = new MongoClient(urlBuilder.ToMongoUrl());
            Database = Client.GetDatabase(urlBuilder.DatabaseName);
        }
    }
}
