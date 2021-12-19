using MongoDB.Bson.Serialization.Attributes;
using System;
using WWA.Grains.Entities;

namespace WWA.Grains.Users.Entities
{
    public class User : Entity
    {
        [BsonElement]
        public string Email { get; set; }
        [BsonElement]
        public string DisplayName { get; set; }
        [BsonElement]
        public string Password { get; set; }
    }
}
