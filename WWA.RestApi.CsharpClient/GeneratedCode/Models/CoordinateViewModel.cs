// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace WWA.RestApi.CsharpClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CoordinateViewModel
    {
        /// <summary>
        /// Initializes a new instance of the CoordinateViewModel class.
        /// </summary>
        public CoordinateViewModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CoordinateViewModel class.
        /// </summary>
        public CoordinateViewModel(int? x = default(int?), int? y = default(int?))
        {
            X = x;
            Y = y;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "x")]
        public int? X { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "y")]
        public int? Y { get; set; }

    }
}