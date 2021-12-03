
namespace WWA.Configuration
{
    public class ApiConfig
    {
        public IdentityConfiguration Identity { get; set; }
        public MongoConfiguration Mongo { get; set; }
        public OrleansConfiguration Orleans { get; set; }
    }
}
