using Newtonsoft.Json;

namespace MapsApiStandardLibrary.Models.Directions
{
    public class Polyline
    {
        [JsonProperty(PropertyName = "points")]
        public string Points { get; set; }
    }
}
