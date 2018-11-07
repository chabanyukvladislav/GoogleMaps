using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using MapsApiLibrary;
using MapsApiLibrary.Api.Parameters.Directions;
using MapsApiLibrary.Api.Parameters.Directions.Enums;
using MapsApiLibrary.Models.Directions;
using Xamarin.Forms;

namespace Maps.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IService<DirectionsParameters, DirectionsResult> _service;

        private ObservableCollection<string> _trafficModels;
        private TrafficModels? _selectedTrafficModel;

        public WaypointsViewModel WaypointsViewModel { get; set; }
        public MapViewModel MapViewModel { get; set; }

        public ObservableCollection<string> TrafficModels
        {
            get => _trafficModels;
            set
            {
                _trafficModels = value;
                OnPropertyChanged(nameof(TrafficModels));
            }
        }
        public TrafficModels? SelectedTrafficModel
        {
            get => _selectedTrafficModel;
            set
            {
                _selectedTrafficModel = value;
                OnPropertyChanged(nameof(SelectedTrafficModel));
            }
        }

        public ICommand CalculateOptimize { get; }

        public MainViewModel()
        {
            _service = new DirectionsService();

            WaypointsViewModel = new WaypointsViewModel();
            MapViewModel = new MapViewModel();

            CalculateOptimize = new Command(ExecuteCalculateOptimize);
        }

        private void SetParameters(bool optimize)
        {
            _service.Parameters.Origin.Latitude = MapViewModel.Pins.StartPin.Coordinate.Latitude;
            _service.Parameters.Origin.Longitude = MapViewModel.Pins.StartPin.Coordinate.Longitude;

            _service.Parameters.Destination.Latitude = MapViewModel.Pins.EndPin.Coordinate.Latitude;
            _service.Parameters.Destination.Longitude = MapViewModel.Pins.EndPin.Coordinate.Longitude;

            _service.Parameters.Key = "AIzaSyA3YhAyyckDAMFGuVR7yRI-fG_NATvL8Yk";

            _service.Parameters.Optimize = optimize;
            foreach (var pin in MapViewModel.Pins.WaypointsPin)
            {
                var location = new Location(pin.Coordinate.Latitude, pin.Coordinate.Longitude);
                _service.Parameters.Waypoints.Add(location);
            }

            if (SelectedTrafficModel != null)
            {
                _service.Parameters.TrafficModel = SelectedTrafficModel;
            }
        }

        private async void ExecuteCalculateOptimize()
        {
            //XAML????

            if (MapViewModel.Pins.StartPin.Coordinate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must select origin path", "Ok");
                return;
            }
            if (MapViewModel.Pins.EndPin.Coordinate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must select destination path", "Ok");
                return;
            }

            SetParameters(true);
            var result = await _service.GetResultAsync();
            CalculateFinished?.Invoke(result);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<DirectionsResult> CalculateFinished;
    }
}
