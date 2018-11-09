using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Maps.Controls.Models;
using MapsApiLibrary.Models.Directions;

namespace Maps.ViewModels
{
    public interface IPinsViewModel: INotifyPropertyChanged
    {
        ObservableCollection<PinPoint> PinPoints { get; set; }
        PinPoint SelectedPinPoint { get; set; }

        ICommand ListViewTapped { get; }

        void LoadPoints(DirectionsResult result);

        event Action<PinPoint, PinPoint> PinSelecting;
    }
}
