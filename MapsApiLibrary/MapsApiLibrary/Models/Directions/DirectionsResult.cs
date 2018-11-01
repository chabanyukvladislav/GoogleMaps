using Newtonsoft.Json;
using System.Collections.Generic;

namespace MapsApiLibrary.Models.Directions
{
    public class DirectionsResult
    {
        [JsonProperty(PropertyName = "geocoded_waypoints")]
        public List<GeocodedWaypoint> GeocodedWaypoints { get; set; }
        [JsonProperty(PropertyName = "routes")]
        public List<Route> Routes { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}
