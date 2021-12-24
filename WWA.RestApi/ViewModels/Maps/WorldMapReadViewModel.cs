using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WWA.GrainInterfaces.Models;

namespace WWA.RestApi.ViewModels.Maps
{
    public class WorldMapReadViewModel : WorldMapSummaryViewModel
    {
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public MapSizeViewModel Size { get; set; }
        /// <summary>
        /// Elevations key is an int that represents the y index of the corresponding Elevation object.
        /// </summary>
        public Dictionary<string, ElevationViewModel> Elevations { get; set; }
    }
}
