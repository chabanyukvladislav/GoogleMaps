using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using MapsApiLibrary.Api.Parameters.Directions;

namespace Maps.UWP.Renderer.Helpers
{
    internal class LocationsToBasicGeopositions
    {
        public static IEnumerable<BasicGeoposition> Convert(IEnumerable<Location> locations)
        {
            var enumerable = locations.ToList();
            if (enumerable.Count == 0)
            {
                return new List<BasicGeoposition>();
            }
            var positions = new List<BasicGeoposition>();
            foreach (var location in enumerable)
            {
                if (location.Latitude == null || location.Longitude == null)
                {
                    continue;
                }
                var position = new BasicGeoposition
                {
                    Latitude = location.Latitude.Value,
                    Longitude = location.Longitude.Value
                };
                positions.Add(position);
            }

            return positions;
        }
    }
}
