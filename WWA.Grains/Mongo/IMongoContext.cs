using MongoDB.Driver;

namespace WWA.Grains.Mongo
{
    public interface IMongoContext
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }
    }
}
