using System;
using WWA.Grains.Entities;

namespace WWA.Grains.Users.Entities
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
    }
}
