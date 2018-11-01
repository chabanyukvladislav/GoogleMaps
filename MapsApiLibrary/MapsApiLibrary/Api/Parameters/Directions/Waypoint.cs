using MapsApiLibrary.Models.Directions;

namespace MapsApiLibrary.Api.Parameters.Directions
{
    public class Waypoint
    {
        public Location Location { get; set; }
        public static bool? Optimize { get; set; }

        public Waypoint()
        {
            Location = new Location();
        }
    }
}
