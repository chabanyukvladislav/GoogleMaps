using Newtonsoft.Json;
using System.Collections.Generic;

namespace MapsApiStandardLibrary.Models.Directions
{
    public class Route
    {
        [JsonProperty(PropertyName = "bounds")]
        public Bounds Bounds { get; set; }
        [JsonProperty(PropertyName = "copyrights")]
        public string Copyrights { get; set; }
        [JsonProperty(PropertyName = "fare")]
        public Fare Fare { get; set; }
        [JsonProperty(PropertyName = "legs")]
        public List<Leg> Legs { get; set; }
        [JsonProperty(PropertyName = "overview_polyline")]
        public Polyline OverviewPolyline { get; set; }
        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }
        [JsonProperty(PropertyName = "warnings")]
        public List<string> Warnings { get; set; }
        [JsonProperty(PropertyName = "waypoint_order")]
        public List<int> WaypointOrder { get; set; }
    }
}
