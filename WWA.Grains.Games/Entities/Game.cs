using WWA.Grains.Entities;

namespace WWA.Grains.Games.Entities
{
    public class Game : Entity
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string OwnedBy { get; set; }
    }
}
