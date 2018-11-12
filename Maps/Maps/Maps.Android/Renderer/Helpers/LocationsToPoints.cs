using System.Collections.Generic;
using System.Linq;
using Android.Gms.Maps.Model;
using MapsApiStandardLibrary.Models.Directions;

namespace Maps.Droid.Renderer.Helpers
{
    internal static class LocationsToPoints
    {
        public static IList<LatLng> Convert(IEnumerable<Coordinate> points)
        {
            var enumerable = points.ToList();
            if (!enumerable.Any())
            {
                return new List<LatLng>();
            }
            var result = new List<LatLng>();
            foreach (var location in enumerable)
            {
                result.Add(new LatLng(location.Latitude, location.Longitude));
            }

            return result;
        }
    }
}