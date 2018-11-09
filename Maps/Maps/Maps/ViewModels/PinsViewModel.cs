using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Maps.Collections;
using Maps.Controls;
using Maps.Controls.Models;
using Xamarin.Forms;

namespace Maps.ViewModels
{
    public class PinsViewModel : IPinsViewModel
    {
        private readonly SharedMyPins _pins;
        private ObservableCollection<PinPoint> _pinPoints;
        private PinPoint _selectedPinPoint;

        public ObservableCollection<PinPoint> PinPoints
        {
            get => _pinPoints;
            set
            {
                _pinPoints = value;
                OnPropertyChanged(nameof(PinPoints));
            }
        }
        public PinPoint SelectedPinPoint
        {
            get => _selectedPinPoint;
            set
            {
                _selectedPinPoint = value;
                OnPropertyChanged(nameof(SelectedPinPoint));
            }
        }

        public ICommand ListViewTapped { get; }

        public PinsViewModel()
        {
            _pins = SharedMyPins.Get;
            PinPoints = new ObservableCollection<PinPoint>();

            ListViewTapped = new Command(ExecuteListViewTapped);

            _pins.PinAdded += OnPinAdded;
            SharedResult.ResultChanged += OnResultChanged;
        }

        private void ExecuteListViewTapped()
        {
            if (SelectedPinPoint == null)
            {
                return;
            }

            var selectedPin = new MyPin
            {
                MyType = MyPinType.Undefined,
                Coordinate = SelectedPinPoint.Coordinate
            };
            _pins.SelectPin(selectedPin);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnResultChanged()
        {
            PinPoints.Clear();
            var legs = SharedResult.Result.Routes[0].Legs;

            var startPinPoint = new PinPoint
            {
                Number = 0,
                Address = legs[0].StartAddress,
                Coordinate = _pins.Pins.StartPin.Coordinate,
                Distance = 0,
                Duration = 0
            };
            PinPoints.Add(startPinPoint);

            var distance = legs[0].Distance.Value;
            var duration = legs[0].Duration.Value;

            for (var i = 1; i < legs.Count; ++i)
            {
                var pinPoint = new PinPoint
                {
                    Number = i,
                    Address = legs[i].StartAddress,
                    Coordinate = _pins.Pins.WaypointsPin[SharedResult.Result.Routes[0].WaypointOrder[i - 1]].Coordinate,
                    Distance = distance,
                    Duration = duration
                };
                PinPoints.Add(pinPoint);

                distance += legs[i].Distance.Value;
                duration += legs[i].Duration.Value;
            }

            var endPinPoint = new PinPoint
            {
                Number = legs.Count,
                Address = legs[legs.Count - 1].EndAddress,
                Coordinate = _pins.Pins.EndPin.Coordinate,
                Distance = distance,
                Duration = duration
            };
            PinPoints.Add(endPinPoint);
        }
        private void OnPinAdded(MyPin value)
        {
            var pinPoint = new PinPoint
            {
                Coordinate = value.Coordinate,
                Number = PinPoints.Count
            };
            PinPoints.Add(pinPoint);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
