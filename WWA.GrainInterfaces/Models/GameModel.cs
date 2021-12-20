using System;
using System.Collections.Generic;

namespace WWA.GrainInterfaces.Models
{
    public class GameModel : UpdatableEntityModel
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string OwnedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateActive { get; set; }
        public IEnumerable<string> Players { get; set; }
    }
}
