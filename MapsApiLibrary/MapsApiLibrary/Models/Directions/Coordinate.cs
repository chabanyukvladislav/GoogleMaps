using Newtonsoft.Json;

namespace MapsApiLibrary.Models.Directions
{
    public class Coordinate
    {
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public double Longitude { get; set; }

        public Coordinate(double latitude = 0, double longitude = 0)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
