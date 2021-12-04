using System;

namespace WWA.RestApi.ViewModels.Games
{
    public class GameSummaryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
