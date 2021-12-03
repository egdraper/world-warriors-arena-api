using System;

namespace WWA.GrainInterfaces.Models
{
    [Serializable]
    public class UserModel : EntityModel
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public DateTime? DateActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
