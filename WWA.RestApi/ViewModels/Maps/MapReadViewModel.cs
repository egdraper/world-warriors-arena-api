using System;

namespace WWA.RestApi.ViewModels.Maps
{
    public class MapReadViewModel : MapSummaryViewModel
    {
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
