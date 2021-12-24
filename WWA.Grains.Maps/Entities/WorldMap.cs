using WWA.Grains.Entities;

namespace WWA.Grains.Maps.Entities
{
    public class WorldMap : TrackedEntity
    {
        public string? Name { get; set; }
        public string? GameId { get; set; }
        public string? CreatedBy { get; set; }
        public MapSize Size { get; set; }
    }
}
