using System.Collections.Generic;

namespace MapsApiLibrary.Models.Directions
{
    public class GeocodedWaypoint
    {
        public string GeocoderStatus { get; set; }
        public string PlaceId { get; set; }
        public List<string> Types { get; set; }
    }
}
