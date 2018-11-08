using MapsApiLibrary.Models.Directions;

namespace Maps.Controls.Models
{
    public class PinPoint
    {
        public int Number { get; set; }
        public Coordinate Coordinate { get; set; }
        public int Duration { get; set; }
        public int Distance { get; set; }
        public string Address { get; set; }

        public PinPoint()
        {
            Coordinate = new Coordinate();
        }
    }
}
