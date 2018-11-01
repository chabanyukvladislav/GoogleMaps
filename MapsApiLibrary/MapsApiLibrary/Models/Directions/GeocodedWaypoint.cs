using Newtonsoft.Json;
using System.Collections.Generic;

namespace MapsApiLibrary.Models.Directions
{
    public class GeocodedWaypoint
    {
        [JsonProperty(PropertyName = "geocoder_status")]
        public string GeocoderStatus { get; set; }
        [JsonProperty(PropertyName = "place_id")]
        public string PlaceId { get; set; }
        [JsonProperty(PropertyName = "types")]
        public List<string> Types { get; set; }
    }
}
