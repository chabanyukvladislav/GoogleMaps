using System.Collections.Generic;

namespace MapsApiLibrary.Models.Directions
{
    public class DirectionsResult
    {
        public List<GeocodedWaypoint> GeocodedWaypoints { get; set; }
        public List<Route> Routes { get; set; }
        public string Status { get; set; }
    }
}
