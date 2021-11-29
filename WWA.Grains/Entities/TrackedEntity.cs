using System;

namespace WWA.Grains.Entities
{
    public class TrackedEntity : Entity
    {
        public virtual DateTime? DateCreated { get; set; }
        public virtual DateTime? DateModified { get; set; }
    }
}
