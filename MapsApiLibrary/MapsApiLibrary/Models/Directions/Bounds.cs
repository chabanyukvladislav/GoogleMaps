using Newtonsoft.Json;

namespace MapsApiLibrary.Models.Directions
{
    public class Bounds
    {
        [JsonProperty(PropertyName = "northeast")]
        public Location Northeast { get; set; }
        [JsonProperty(PropertyName = "southwest")]
        public Location Southwest { get; set; }
    }
}
