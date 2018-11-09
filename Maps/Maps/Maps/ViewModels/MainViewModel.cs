using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Maps.Collections;
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
        private readonly SharedMyPins _myPins;

        private ObservableCollection<string> _trafficModels;
        private TrafficModels _selectedTrafficModel;

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
        public string SelectedTrafficModel
        {
            get => ConvertEnumToString(_selectedTrafficModel);
            set
            {
                _selectedTrafficModel = ConvertStringToEnum(value);
                OnPropertyChanged(nameof(SelectedTrafficModel));
            }
        }

        public Command Calculate { get; }

        public MainViewModel()
        {
            _trafficModels = new ObservableCollection<string>(Enum.GetNames(typeof(TrafficModels)));

            _service = new DirectionsService();
            _myPins = SharedMyPins.Get;

            PinsViewModel = new PinsViewModel();
            MapViewModel = new MapViewModel();

            Calculate = new Command(ExecuteCalculate, IsButtonEnabled);

            OnPinsChanged();
        }

        public static string ConvertEnumToString(Enum eEnum)
        {
            return Enum.GetName(eEnum.GetType(), eEnum);
        }
        public static TrafficModels ConvertStringToEnum(string value)
        {
            return Enum.Parse<TrafficModels>(value);
        }

        private bool IsButtonEnabled(object arg)
        {
            return _myPins.Pins.StartPin.Coordinate != null && _myPins.Pins.EndPin.Coordinate != null;
        }
        private void CanCalculateUpdate(MyPin pin, Coordinate coordinate)
        {
            Calculate.ChangeCanExecute();
        }

        private async void ExecuteCalculate(object optimize)
        {
            DirectionParametersSetter.Set(ref _service, _myPins.Pins, bool.Parse(optimize.ToString()), _selectedTrafficModel);
            var result = await _service.GetResultAsync();
            if (result.Status != "OK")
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Zero result", "Ok");
                return;
            }

            SharedResult.Result = result;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPinsChanged()
        {
            _myPins.Pins.StartPin.CoordinateChanged += CanCalculateUpdate;
            _myPins.Pins.EndPin.CoordinateChanged += CanCalculateUpdate;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
