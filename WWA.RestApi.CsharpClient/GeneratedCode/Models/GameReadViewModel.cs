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

    public partial class GameReadViewModel
    {
        /// <summary>
        /// Initializes a new instance of the GameReadViewModel class.
        /// </summary>
        public GameReadViewModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GameReadViewModel class.
        /// </summary>
        public GameReadViewModel(string id = default(string), string name = default(string), string ownedBy = default(string), IList<string> players = default(IList<string>), string createdBy = default(string))
        {
            Id = id;
            Name = name;
            OwnedBy = ownedBy;
            Players = players;
            CreatedBy = createdBy;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ownedBy")]
        public string OwnedBy { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "players")]
        public IList<string> Players { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

    }
}
