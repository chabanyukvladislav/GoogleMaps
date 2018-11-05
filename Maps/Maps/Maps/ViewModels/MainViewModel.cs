using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Maps.Controls.Models;
using Maps.Models.Controls;
using MapsApiLibrary;
using MapsApiLibrary.Api.Parameters.Directions;
using MapsApiLibrary.Models.Directions;
using Xamarin.Forms;

namespace Maps.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MyPin> _pins;
        private readonly DirectionsService _service;

        public ObservableCollection<MyPin> Pins
        {
            get => _pins;
            set
            {
                _pins = value;
                OnPropertyChanged(nameof(Pins));
            }
        }
        public ICommand Calculate { get; }

        public MainViewModel()
        {
            _service = new DirectionsService();
            Pins = new ObservableCollection<MyPin>();
            Calculate = new Command(ExecuteCalculate);
        }

        private async void ExecuteCalculate()
        {
            var origin = Pins.FirstOrDefault(el => el.MyType == MyPinType.Start);
            if (origin == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must select origin path", "Ok");
                return;
            }
            _service.Parameters.Origin.Latitude = origin.Position.Latitude;
            _service.Parameters.Origin.Longitude = origin.Position.Longitude;

            var destination = Pins.FirstOrDefault(el => el.MyType == MyPinType.End);
            if (destination == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must select destination path", "Ok");
                return;
            }
            _service.Parameters.Destination.Latitude = destination.Position.Latitude;
            _service.Parameters.Destination.Longitude = destination.Position.Longitude;

            _service.Parameters.Key = "AIzaSyA3YhAyyckDAMFGuVR7yRI-fG_NATvL8Yk";

            _service.Parameters.DepartureTime = DateTime.Parse("11/06/2018 16:00");

            if (Pins.Count > 2)
            {
                foreach (var pin in Pins)
                {
                    if (pin.MyType == MyPinType.Start)
                    {
                        continue;
                    }

                    if (pin.MyType == MyPinType.End)
                    {
                        continue;
                    }

                    var location = new Location(pin.Position.Latitude, pin.Position.Longitude);
                    _service.Parameters.Waypoints.Add(location);
                }
            }

            var result = await _service.GetResult();
            var steps = new List<Step>();
            foreach (var route in result.Routes)
            {
                foreach (var leg in route.Legs)
                {
                    foreach (var step in leg.Steps)
                    {
                        steps.Add(step);
                    }
                }
            }
            PathRender?.Invoke(steps);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<List<Step>> PathRender;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
