using System.ComponentModel;
using Maps.Controls;

namespace Maps.ViewModels
{
    public interface IMapViewModel: INotifyPropertyChanged
    {
        MyPins Pins { get; set; }
    }
}
