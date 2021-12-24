using System;

namespace WWA.GrainInterfaces.Models
{
    public class WorldMapModel : MapModel
    {
        public string Name { get; set; }
        public string GameId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
