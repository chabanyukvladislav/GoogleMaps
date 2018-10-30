namespace MapsApiLibrary.Models.Directions
{
    public class Step
    {
        public TextValuePair Distance { get; set; }
        public TextValuePair Duration { get; set; }
        public Location EndLocation { get; set; }
        public string HtmlInstructions { get; set; }
        public string Maneuver { get; set; }
        public Polyline Polyline { get; set; }
        public Location StartLocation { get; set; }
        public string TravelMode { get; set; }
    }
}
