
namespace WWA.Configuration
{
    public class ApiConfig
    {
        public const string Section = "restapi";
        public MongoConfiguration Mongo { get; set; }
    }
}
