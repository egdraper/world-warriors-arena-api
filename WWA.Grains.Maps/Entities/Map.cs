using WWA.Grains.Entities;

namespace WWA.Grains.Maps.Entities
{
    public class Map : TrackedEntity
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
    }
}
