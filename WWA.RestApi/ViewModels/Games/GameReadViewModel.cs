namespace WWA.RestApi.ViewModels.Games
{
    public class GameReadViewModel : GameSummaryViewModel
    {
        public string CreatedBy { get; set; }
        public string OwnedBy { get; set; }
    }
}
