
namespace WWA.Configuration
{
    public class IdentityConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int TokenExpiryInMinutes { get; set; }
    }
}
