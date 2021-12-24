using System.Collections.Generic;

namespace WWA.GrainInterfaces.Models
{
    public class WorldMapUpdateModel
    {
        public string Name { get; set; }
        public Dictionary<string, MapElevationModel> Elevations { get; set; }
    }
}
