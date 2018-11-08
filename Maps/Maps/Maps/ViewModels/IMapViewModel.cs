using System;
using System.ComponentModel;
using Maps.Controls;
using Maps.Controls.Models;
using MapsApiLibrary.Models.Directions;

namespace Maps.ViewModels
{
    public interface IMapViewModel: INotifyPropertyChanged
    {
        MyPins Pins { get; set; }

        void RenderPath(DirectionsResult result);
        void PinSelect(PinPoint newPin, PinPoint oldPin);
        void PinsUpdate(MyPins pins);

        event Action<string> PathRendering;
        event Action PinsUpdated;
    }
}
