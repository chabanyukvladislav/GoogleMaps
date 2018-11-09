using System;
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

        public override bool Equals(object value)
        {
            if (!(value is Coordinate coordinate))
            {
                return false;
            }
            var latitudeEquals = Math.Abs(Latitude - coordinate.Latitude) <= 0.00002;
            var longitudeEquals = Math.Abs(Longitude - coordinate.Longitude) <= 0.00002;
            return latitudeEquals && longitudeEquals;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
