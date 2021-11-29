using System;
using WWA.Grains.Entities;

namespace WWA.Grains.Users.Entities
{
    public class UserState : TrackedEntity
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public DateTime DateActive { get; set; }
    }
}
