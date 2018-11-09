using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using MapsApiLibrary.Models.Directions;

namespace Maps.UWP.Renderer.Helpers
{
    internal class LocationsToBasicGeopositions
    {
        public static IEnumerable<BasicGeoposition> Convert(IEnumerable<Coordinate> locations)
        {
            var enumerable = locations.ToList();
            if (enumerable.Count == 0)
            {
                return new List<BasicGeoposition>();
            }
            var positions = new List<BasicGeoposition>();
            foreach (var location in enumerable)
            {
                var position = new BasicGeoposition
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                };
                positions.Add(position);
            }

            return positions;
        }
    }
}
