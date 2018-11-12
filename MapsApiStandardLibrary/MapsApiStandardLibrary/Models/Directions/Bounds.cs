using Newtonsoft.Json;

namespace MapsApiStandardLibrary.Models.Directions
{
    public class Bounds
    {
        [JsonProperty(PropertyName = "northeast")]
        public Coordinate Northeast { get; set; }
        [JsonProperty(PropertyName = "southwest")]
        public Coordinate Southwest { get; set; }
    }
}
