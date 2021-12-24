// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace WWA.RestApi.CsharpClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class TerrainCellViewModel
    {
        /// <summary>
        /// Initializes a new instance of the TerrainCellViewModel class.
        /// </summary>
        public TerrainCellViewModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TerrainCellViewModel class.
        /// </summary>
        /// <param name="terrainType">Possible values include: 'RaisedBase',
        /// 'RaisedNonBase', 'RecessedVoid', 'RecessedFilled'</param>
        public TerrainCellViewModel(string spriteId = default(string), CoordinateViewModel tileLocation = default(CoordinateViewModel), int? z = default(int?), bool? isObstructed = default(bool?), string terrainType = default(string))
        {
            SpriteId = spriteId;
            TileLocation = tileLocation;
            Z = z;
            IsObstructed = isObstructed;
            TerrainType = terrainType;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "spriteId")]
        public string SpriteId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "tileLocation")]
        public CoordinateViewModel TileLocation { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "z")]
        public int? Z { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isObstructed")]
        public bool? IsObstructed { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'RaisedBase',
        /// 'RaisedNonBase', 'RecessedVoid', 'RecessedFilled'
        /// </summary>
        [JsonProperty(PropertyName = "terrainType")]
        public string TerrainType { get; set; }

    }
}
