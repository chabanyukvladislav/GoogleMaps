using System.Collections.Generic;

namespace MapsApiLibrary.Models.Directions
{
    public class Route
    {
        public Bounds Bounds { get; set; }
        public string Copyrights { get; set; }
        public List<Leg> Legs { get; set; }
        public Polyline OverviewPolyline { get; set; }
        public string Summary { get; set; }
        public List<int> WaypointOrders { get; set; }
    }
}
