using System.Collections.Generic;

namespace WWA.RestApi.ViewModels.Games
{
    public class GameUpdateViewModel
    {
        public string Name { get; set; }
        public string OwnedBy { get; set; }
        public IEnumerable<string> Players { get; set; }
    }
}
