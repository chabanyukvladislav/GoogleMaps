using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Maps.Content;
using Maps.Controls.Models;
using Maps.Models.Controls;
using MapsApiLibrary;
using MapsApiLibrary.Api.Parameters.Directions;
using MapsApiLibrary.Models.Directions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MyPin> _pins;
        private readonly IService<DirectionsParameters, DirectionsResult> _service;

        private bool _optimize;
        private ObservableCollection<Location> _waypoints;
        private Location _selected;

        public ObservableCollection<MyPin> Pins
        {
            get => _pins;
            set
            {
                _pins = value;
                OnPropertyChanged(nameof(Pins));
            }
        }
        public ObservableCollection<Location> Waypoints
        {
            get => _waypoints;
            set
            {
                _waypoints = value;
                OnPropertyChanged(nameof(Waypoints));
            }
        }
        public Location SelectedPoint
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(SelectedPoint));
            }
        }

        public ICommand Calculate { get; }
        public ICommand ListViewTapped { get; }

        public MainViewModel()
        {
            _optimize = true;
            _service = new DirectionsService();
            Pins = new ObservableCollection<MyPin>();
            Calculate = new Command(ExecuteCalculate);
            Waypoints = new ObservableCollection<Location>();
            ListViewTapped = new Command(ExecuteListViewTapped);
        }

        private void ExecuteListViewTapped()
        {
            if (SelectedPoint?.Latitude == null || SelectedPoint.Longitude == null)
            {
                return;
            }

            foreach (var pin in Pins)
            {
                if (pin.MyType == MyPinType.Start || pin.MyType == MyPinType.End)
                {
                    continue;
                }
                if (Math.Abs(pin.Position.Longitude - SelectedPoint.Longitude.Value) <= 0.00002 &&
                    Math.Abs(pin.Position.Latitude - SelectedPoint.Latitude.Value) <= 0.00002)
                {
                    pin.IconPath = IconsPath.SelectedPin;
                    continue;
                }
                if(pin.MyType == MyPinType.Waypoint)
                {
                    pin.IconPath = IconsPath.WaypointPin;
                }
            }
            OnPropertyChanged(nameof(Pins));
        }

        private async void ExecuteCalculate()
        {
            var origin = Pins.FirstOrDefault(el => el.MyType == MyPinType.Start);
            if (origin == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must select origin path", "Ok");
                return;
            }
            var destination = Pins.FirstOrDefault(el => el.MyType == MyPinType.End);
            if (destination == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must select destination path", "Ok");
                return;
            }

            AddParameters(origin, destination);

            var result = await _service.GetResultAsync();
            PathRender?.Invoke(result.Routes[0].OverviewPolyline.Points);

            var collection = GetOrderPinsLocation(result);
            Waypoints = new ObservableCollection<Location>(collection);
            _service.Parameters.Clear();
        }

        private static IEnumerable<Location> GetOrderPinsLocation(DirectionsResult result)
        {
            if (result.Routes[0].WaypointOrder.Count == 0)
            {
                return new List<Location>();
            }

            var waypoints = result.GeocodedWaypoints;
            waypoints.RemoveAt(0);
            waypoints.RemoveAt(waypoints.Count - 1);
            var orderPlaceId = new List<string>();
            var locations = new List<Location>();
            foreach (var i in result.Routes[0].WaypointOrder)
            {
                orderPlaceId.Add(waypoints[i].PlaceId);
            }

            for (var i = 0; i < result.Routes[0].Legs.Count - 1; ++i)
            {
                var location = new Location
                {
                    Latitude = result.Routes[0].Legs[i].EndLocation.Latitude,
                    Longitude = result.Routes[0].Legs[i].EndLocation.Longitude,
                    PlaceId = orderPlaceId[i]
                };
                locations.Add(location);
            }

            return locations;
        }

        private void AddParameters(Pin origin, Pin destination)
        {
            _service.Parameters.Origin.Latitude = origin.Position.Latitude;
            _service.Parameters.Origin.Longitude = origin.Position.Longitude;

            _service.Parameters.Destination.Latitude = destination.Position.Latitude;
            _service.Parameters.Destination.Longitude = destination.Position.Longitude;

            _service.Parameters.Key = "AIzaSyA3YhAyyckDAMFGuVR7yRI-fG_NATvL8Yk";
            _service.Parameters.Optimize = _optimize;

            if (Pins.Count <= 2)
            {
                return;
            }
            foreach (var pin in Pins)
            {
                if (pin.MyType == MyPinType.Start || pin.MyType == MyPinType.End)
                {
                    continue;
                }

                var location = new Location(pin.Position.Latitude, pin.Position.Longitude);
                _service.Parameters.Waypoints.Add(location);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string> PathRender;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
