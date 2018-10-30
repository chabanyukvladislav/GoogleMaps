using MapsApiLibrary.Api.Parametrs.Directions.Enums;
using System;
using System.Collections.Generic;

namespace MapsApiLibrary.Api.Parametrs.Directions
{
    public class DirectionsParametrs
    {
        //public string Origin { get; set; }
        //public string Destination { get; set; }
        public string Key { get; set; }
        public Modes Mode { get; set; }
        //public List<string> Waypoints { get; set; }
        public bool Alternatives { get; set; }
        public Avoids Avoid { get; set; }
        //public string Language { get; set; }
        public Units Units { get; set; }
        //public string Region { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public TrafficModels TrafficModel { get; set; }
        public TransitModes TransitMode { get; set; }
        public TransitRoutingPreferences TransitRoutingPreference { get; set; }
    }
}
