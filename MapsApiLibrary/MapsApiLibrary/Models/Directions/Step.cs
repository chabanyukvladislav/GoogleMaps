using Newtonsoft.Json;
using System.Collections.Generic;

namespace MapsApiLibrary.Models.Directions
{
    public class Step
    {
        [JsonProperty(PropertyName = "distance")]
        public TextValuePair Distance { get; set; }
        [JsonProperty(PropertyName = "duration")]
        public TextValuePair Duration { get; set; }
        [JsonProperty(PropertyName = "end_location")]
        public Coordinate EndLocation { get; set; }
        [JsonProperty(PropertyName = "html_instructions")]
        public string HtmlInstructions { get; set; }
        [JsonProperty(PropertyName = "maneuver")]
        public string Maneuver { get; set; }
        [JsonProperty(PropertyName = "polyline")]
        public Polyline Polyline { get; set; }
        [JsonProperty(PropertyName = "start_location")]
        public Coordinate StartLocation { get; set; }
        [JsonProperty(PropertyName = "steps")]
        public List<Step> Steps { get; set; }//???
        [JsonProperty(PropertyName = "travel_mode")]
        public string TravelMode { get; set; }
    }
}
