using Newtonsoft.Json;
using System.Collections.Generic;

namespace MapsApiLibrary.Models.Directions
{
    public class Leg
    {
        [JsonProperty(PropertyName = "arrival_time")]
        public Time ArrivalTime { get; set; }
        [JsonProperty(PropertyName = "departure_time")]
        public Time DepartureTime { get; set; }
        [JsonProperty(PropertyName = "distance")]
        public TextValuePair Distance { get; set; }
        [JsonProperty(PropertyName = "duration")]
        public TextValuePair Duration { get; set; }
        [JsonProperty(PropertyName = "end_address")]
        public string EndAddress { get; set; }
        [JsonProperty(PropertyName = "end_location")]
        public Coordinate EndLocation { get; set; }
        [JsonProperty(PropertyName = "start_address")]
        public string StartAddress { get; set; }
        [JsonProperty(PropertyName = "start_location")]
        public Coordinate StartLocation { get; set; }
        [JsonProperty(PropertyName = "steps")]
        public List<Step> Steps { get; set; }
    }
}
