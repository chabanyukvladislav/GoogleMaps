using Newtonsoft.Json;

namespace MapsApiLibrary.Models.Directions
{
    public class Time
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "time_zone")]
        public string TimeZone { get; set; }
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
    }
}