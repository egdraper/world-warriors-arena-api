using WWA.Grains.Entities;

namespace WWA.Grains.Maps.Entities
{
    public class WorldMapState : MapState
    {
        public string? Name { get; set; }
        public string? CreatedBy { get; set; }
        public string? GameId { get; set; }
    }
}
