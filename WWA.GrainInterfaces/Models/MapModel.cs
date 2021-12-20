using System;

namespace WWA.GrainInterfaces.Models
{
    public class MapModel : EntityModel
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
