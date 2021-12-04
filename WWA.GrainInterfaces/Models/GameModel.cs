using System;

namespace WWA.GrainInterfaces.Models
{
    public class GameModel : EntityModel
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string OwnedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
