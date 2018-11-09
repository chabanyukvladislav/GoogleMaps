using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Maps.Controls.Models;
using MapsApiLibrary.Models.Directions;
using Xamarin.Forms;

namespace Maps.ViewModels
{
    public class PinsViewModel : IPinsViewModel
    {
        private ObservableCollection<PinPoint> _pinPoints;
        private PinPoint _selectedPinPoint;
        private PinPoint _lastSelected;

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
            PinPoints = new ObservableCollection<PinPoint>();

            ListViewTapped = new Command(ExecuteListViewTapped);
        }

        public void LoadPoints(DirectionsResult result)
        {
            PinPoints.Clear();
            var legs = result.Routes[0].Legs;
            var distance = 0;
            var duration = 0;
            for (var i = 0; i < legs.Count; ++i)
            {
                var pinPoint = new PinPoint
                {
                    Number = i,
                    Address = legs[i].StartAddress,
                    Coordinate = legs[i].StartLocation,
                    Distance = distance,
                    Duration = duration
                };
                PinPoints.Add(pinPoint);

                distance += legs[i].Distance.Value;
                duration += legs[i].Duration.Value;

                if (i != legs.Count - 1)
                {
                    continue;
                }
                pinPoint = new PinPoint
                {
                    Number = i + 1,
                    Address = legs[i].EndAddress,
                    Coordinate = legs[i].EndLocation,
                    Distance = distance,
                    Duration = duration
                };
                PinPoints.Add(pinPoint);
            }
        }

        private void ExecuteListViewTapped()
        {
            if (SelectedPinPoint == null && _lastSelected == null)
            {
                return;
            }

            PinSelecting?.Invoke(SelectedPinPoint, _lastSelected);

            _lastSelected = SelectedPinPoint;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<PinPoint, PinPoint> PinSelecting;
    }
}
