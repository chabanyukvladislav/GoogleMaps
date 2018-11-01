using System;
using System.Globalization;

namespace MapsApiLibrary.Api.Parameters.Directions
{
    public class Location
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string PlaceId { get; set; }

        public Location() { }
        public Location(string placeId)
        {
            PlaceId = placeId;
        }
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public static implicit operator string(Location loc)
        {
            if (loc == null)
            {
                return "";
            }

            var location = "";

            if (loc.Longitude != null && loc.Latitude != null)
            {
                location = $"{loc.Latitude.Value.ToString(CultureInfo.InvariantCulture).Replace(',', '.')}," +
                           $"{loc.Longitude.Value.ToString(CultureInfo.InvariantCulture).Replace(',', '.')}";
            }
            else if (string.IsNullOrWhiteSpace(loc.PlaceId))
            {
                location = $"place_id:{loc.PlaceId}";
            }

            return location;
        }
        public static implicit operator Location(string loc)
        {
            if (string.IsNullOrWhiteSpace(loc))
            {
                return new Location();
            }

            var location = new Location();
            if (loc.IndexOf("place_id", StringComparison.Ordinal) != -1)
            {
                var placeId = loc.Split(':')[1];
                location.PlaceId = placeId;
                return location;
            }

            var locations = loc.Split(',');
            location.Latitude = Convert.ToDouble(locations[0]);
            location.Longitude = Convert.ToDouble(locations[1]);
            return location;
        }
    }
}
