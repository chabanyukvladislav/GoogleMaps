using System;
using System.ComponentModel;
using Maps.Controls;

namespace Maps.ViewModels
{
    public class MapViewModel : INotifyPropertyChanged
    {
        private MyPins _pins;

        public MyPins Pins
        {
            get => _pins;
            set
            {
                _pins = value;
                OnPropertyChanged(nameof(Pins));
            }
        }

        public MapViewModel()
        {
            Pins = new MyPins();
        }

        //private static IEnumerable<Location> GetOrderPinsLocation(DirectionsResult result)
        //{
        //    if (result.Routes[0].WaypointOrder.Count == 0)
        //    {
        //        return new List<Location>();
        //    }

        //    var waypoints = result.GeocodedWaypoints;
        //    waypoints.RemoveAt(0);
        //    waypoints.RemoveAt(waypoints.Count - 1);
        //    var orderPlaceId = new List<string>();
        //    var locations = new List<Location>();
        //    foreach (var i in result.Routes[0].WaypointOrder)
        //    {
        //        orderPlaceId.Add(waypoints[i].PlaceId);
        //    }

        //    for (var i = 0; i < result.Routes[0].Legs.Count - 1; ++i)
        //    {
        //        var location = new Location
        //        {
        //            Latitude = result.Routes[0].Legs[i].EndLocation.Latitude,
        //            Longitude = result.Routes[0].Legs[i].EndLocation.Longitude,
        //            PlaceId = orderPlaceId[i]
        //        };
        //        locations.Add(location);
        //    }

        //    return locations;
        //}

        //public event Action<string> PathRender;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string> PathRender;
    }
}
