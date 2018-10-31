﻿using System;
using System.Globalization;

namespace MapsApiLibrary.Models.Directions
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public static implicit operator string(Location loc)
        {
            if (loc == null)
            {
                return "";
            }

            var location = string.Format($"{0},{1}",
                loc.Latitude.ToString(CultureInfo.InvariantCulture).Replace(',', '.'),
                loc.Longitude.ToString(CultureInfo.InvariantCulture).Replace(',', '.'));
            return location;
        }
        public static implicit operator Location(string loc)
        {
            if (string.IsNullOrWhiteSpace(loc))
            {
                return new Location();
            }

            var location = new Location();
            var locations = loc.Split(',');
            location.Latitude = Convert.ToDouble(locations[0]);
            location.Longitude = Convert.ToDouble(locations[1]);
            return location;
        }
    }
}
