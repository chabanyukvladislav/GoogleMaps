using Newtonsoft.Json;

namespace MapsApiStandardLibrary.Models.Directions
{
    public class Fare
    {
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "value")]
        public double Value { get; set; }
    }
}
