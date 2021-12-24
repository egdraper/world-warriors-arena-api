using System.Collections.Generic;

namespace WWA.RestApi.ViewModels.Maps
{
    public class WorldMapUpdateViewModel
    {
        public string Name { get; set; }
        /// <summary>
        /// Elevations key is an int that represents the y index of the corresponding Elevation object.
        /// </summary>
        public Dictionary<int, ElevationViewModel> Elevations { get; set; }
    }
}
