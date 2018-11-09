using System;
using System.ComponentModel;
using System.Linq;
using Maps.Content;
using Maps.Controls;
using Maps.Controls.Models;
using MapsApiLibrary.Models.Directions;

namespace Maps.ViewModels
{
    public class MapViewModel : IMapViewModel
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

        public void RenderPath(DirectionsResult result)
        {
            PathRendering?.Invoke(result.Routes[0].OverviewPolyline.Points);
        }
        public void PinSelect(PinPoint newPin, PinPoint oldPin)
        {
            if (oldPin != null)
            {
                UnPined(oldPin);
            }

            if (newPin == null)
            {
                return;
            }

            Pined(newPin);
        }
        public void PinsUpdate(MyPins pins)
        {
            Pins = pins;
        }

        private void Pined(PinPoint newPin)
        {
            if (newPin.Number == 0)
            {
                Pins.StartPin.IconPath = IconsPath.SelectedPin;
            }
            else if (newPin.Number == Pins.WaypointsPin.Count + 1)
            {
                Pins.EndPin.IconPath = IconsPath.SelectedPin;
            }
            else
            {
                var element = Pins.WaypointsPin.FirstOrDefault(el => el.Coordinate.Equals(newPin.Coordinate));
                if (element != null)
                {
                    element.IconPath = IconsPath.SelectedPin;
                }
            }
        }
        private void UnPined(PinPoint oldPin)
        {
            if (oldPin.Number == 0)
            {
                Pins.StartPin.IconPath = IconsPath.StartEndPin;
            }
            else if (oldPin.Number == Pins.WaypointsPin.Count + 1)
            {
                Pins.EndPin.IconPath = IconsPath.StartEndPin;
            }
            else
            {
                var element = Pins.WaypointsPin.FirstOrDefault(el => el.Coordinate.Equals(oldPin.Coordinate));
                if (element != null)
                {
                    element.IconPath = IconsPath.WaypointPin;
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(Pins))
            {
                PinsUpdated?.Invoke();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string> PathRendering;
        public event Action PinsUpdated;
    }
}
