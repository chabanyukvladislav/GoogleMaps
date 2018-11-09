using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Maps.Controls;
using Maps.Helpers;
using MapsApiLibrary;
using MapsApiLibrary.Api.Parameters.Directions;
using MapsApiLibrary.Api.Parameters.Directions.Enums;
using MapsApiLibrary.Models.Directions;
using Xamarin.Forms;

namespace Maps.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IService<DirectionsParameters, DirectionsResult> _service;

        private ObservableCollection<string> _trafficModels;
        private TrafficModels? _selectedTrafficModel;

        public IPinsViewModel PinsViewModel { get; set; }
        public IMapViewModel MapViewModel { get; set; }

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

        public Command Calculate { get; }

        public MainViewModel()
        {
            _service = new DirectionsService();

            PinsViewModel = new PinsViewModel();
            MapViewModel = new MapViewModel();

            Calculate = new Command(ExecuteCalculate, IsButtonEnabled);

            CalculateFinished += MapViewModel.RenderPath;
            CalculateFinished += PinsViewModel.LoadPoints;

            PinsViewModel.PinSelecting += MapViewModel.PinSelect;

            MapViewModel.PinsUpdated += OnUpdatedEvent;
            OnUpdatedEvent();
        }

        private bool IsButtonEnabled(object arg)
        {
            return MapViewModel.Pins.StartPin.Coordinate != null && MapViewModel.Pins.EndPin.Coordinate != null;
        }
        private void CanCalculateUpdate(MyPin pin, Coordinate coordinate)
        {
            Calculate.ChangeCanExecute();
        }

        private async void ExecuteCalculate(object optimize)
        {
            DirectionParametersSetter.Set(ref _service, MapViewModel.Pins, bool.Parse(optimize.ToString()), SelectedTrafficModel);
            var result = await _service.GetResultAsync();
            if (result.Status != "OK")
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Zero result", "Ok");
                return;
            }
            CalculateFinished?.Invoke(result);
        }
        
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnUpdatedEvent()
        {
            MapViewModel.Pins.StartPin.CoordinateChanged += CanCalculateUpdate;
            MapViewModel.Pins.EndPin.CoordinateChanged += CanCalculateUpdate;
            Calculate.ChangeCanExecute();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<DirectionsResult> CalculateFinished;
    }
}
