// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace WWA.RestApi.CsharpClient.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class SpriteLayerReadViewModel
    {
        /// <summary>
        /// Initializes a new instance of the SpriteLayerReadViewModel class.
        /// </summary>
        public SpriteLayerReadViewModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the SpriteLayerReadViewModel class.
        /// </summary>
        /// <param name="grid">Grid key represents a coordinate and should be
        /// formatted as "{int x}:{int y}".</param>
        public SpriteLayerReadViewModel(IDictionary<string, CellReadViewModel> grid = default(IDictionary<string, CellReadViewModel>))
        {
            Grid = grid;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets grid key represents a coordinate and should be
        /// formatted as "{int x}:{int y}".
        /// </summary>
        [JsonProperty(PropertyName = "grid")]
        public IDictionary<string, CellReadViewModel> Grid { get; set; }

    }
}