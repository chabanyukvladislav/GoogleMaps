using Newtonsoft.Json;

namespace MapsApiLibrary.Models.Directions
{
    public class Bounds
    {
        [JsonProperty(PropertyName = "northeast")]
        public Coordinate Northeast { get; set; }
        [JsonProperty(PropertyName = "southwest")]
        public Coordinate Southwest { get; set; }
    }
}
