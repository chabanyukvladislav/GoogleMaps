using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Maps.Controls.Models;

namespace Maps.ViewModels
{
    public interface IPinsViewModel: INotifyPropertyChanged
    {
        ObservableCollection<PinPoint> PinPoints { get; set; }
        PinPoint SelectedPinPoint { get; set; }

        ICommand ListViewTapped { get; }
    }
}
