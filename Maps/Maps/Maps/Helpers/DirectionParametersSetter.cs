using System;
using Maps.Controls;
using MapsApiLibrary;
using MapsApiLibrary.Api.Parameters.Directions;
using MapsApiLibrary.Api.Parameters.Directions.Enums;
using MapsApiLibrary.Models.Directions;

namespace Maps.Helpers
{
    internal static class DirectionParametersSetter
    {
        public static void Set(ref IService<DirectionsParameters, DirectionsResult> service, MyPins pins, bool optimize = true, TrafficModels? trafficModels = null)
        {
            service.Parameters.Origin.Latitude = pins.StartPin.Coordinate.Latitude;
            service.Parameters.Origin.Longitude = pins.StartPin.Coordinate.Longitude;

            service.Parameters.Destination.Latitude = pins.EndPin.Coordinate.Latitude;
            service.Parameters.Destination.Longitude = pins.EndPin.Coordinate.Longitude;

            service.Parameters.Key = "AIzaSyA3YhAyyckDAMFGuVR7yRI-fG_NATvL8Yk";

            service.Parameters.Optimize = optimize;
            foreach (var pin in pins.WaypointsPin)
            {
                var location = new Location(pin.Coordinate.Latitude, pin.Coordinate.Longitude);
                service.Parameters.Waypoints.Add(location);
            }

            if (trafficModels == null || trafficModels.Value == TrafficModels.BestGuess)
            {
                return;
            }
            service.Parameters.TrafficModel = trafficModels;
            service.Parameters.DepartureTime = DateTime.Now.AddMinutes(1);
        }
    }
}
