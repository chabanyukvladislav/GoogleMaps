using Newtonsoft.Json;

namespace MapsApiLibrary.Models.Directions
{
    public class Coordinate
    {
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }
    }
}
