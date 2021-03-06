﻿using Newtonsoft.Json;

namespace MapsApiLibrary.Models.Directions
{
    public class TextValuePair
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
    }
}
