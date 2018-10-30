using System.Collections.Generic;

namespace MapsApiLibrary.Models.Directions
{
    public class Leg
    {
        public TextValuePair Distance { get; set; }
        public TextValuePair Duration { get; set; }
        public string EndAddress { get; set; }
        public Location EndLocation { get; set; }
        public string StartAddress { get; set; }
        public Location StartLocation { get; set; }
        public List<Step> Steps { get; set; }
    }
}
