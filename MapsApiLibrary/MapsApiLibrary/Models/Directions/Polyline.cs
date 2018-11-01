using Newtonsoft.Json;

namespace MapsApiLibrary.Models.Directions
{
    public class Polyline
    {
        [JsonProperty(PropertyName = "points")]
        public string Points { get; set; }
    }
}
