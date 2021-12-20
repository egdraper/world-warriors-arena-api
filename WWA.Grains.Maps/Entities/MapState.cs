using WWA.Grains.Entities;

namespace WWA.Grains.Maps.Entities
{
    public class MapState : TrackedEntity
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
    }
}
