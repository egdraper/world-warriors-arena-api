// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace WWA.RestApi.CsharpClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ObjectCellViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ObjectCellViewModel class.
        /// </summary>
        public ObjectCellViewModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ObjectCellViewModel class.
        /// </summary>
        public ObjectCellViewModel(string spriteId = default(string), CoordinateViewModel tileLocation = default(CoordinateViewModel), int? z = default(int?), bool? isObstructed = default(bool?), string objectKey = default(string))
        {
            SpriteId = spriteId;
            TileLocation = tileLocation;
            Z = z;
            IsObstructed = isObstructed;
            ObjectKey = objectKey;
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
        /// </summary>
        [JsonProperty(PropertyName = "objectKey")]
        public string ObjectKey { get; set; }

    }
}