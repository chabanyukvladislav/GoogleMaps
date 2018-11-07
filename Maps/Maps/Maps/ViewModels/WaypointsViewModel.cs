using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using MapsApiLibrary.Models.Directions;
using Xamarin.Forms;

namespace Maps.ViewModels
{
    public class WaypointsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Leg> _legs;
        private Leg _selectedLeg;

        public ObservableCollection<Leg> Legs
        {
            get => _legs;
            set
            {
                _legs = value;
                if (_legs?.Count != 0)
                {
                    _legs?.RemoveAt(_legs.Count - 1);
                }
                OnPropertyChanged(nameof(Legs));
            }
        }
        public Leg SelectedLeg
        {
            get => _selectedLeg;
            set
            {
                _selectedLeg = value;
                OnPropertyChanged(nameof(SelectedLeg));
            }
        }

        public ICommand ListViewTapped { get; }

        public WaypointsViewModel()
        {
            Legs = new ObservableCollection<Leg>();

            //ListViewTapped = new Command(ExecuteListViewTapped);
        }

        //private void ExecuteListViewTapped()
        //{
        //    if (SelectedLeg == null)
        //    {
        //        return;
        //    }

        //    //Event???

        //    foreach (var leg in Legs)
        //    {
        //        if (Math.Abs(leg.StartLocation.Longitude - SelectedLeg.StartLocation.Longitude) <= 0.00002 &&
        //            Math.Abs(leg.StartLocation.Latitude - SelectedLeg.StartLocation.Latitude) <= 0.00002)
        //        {
        //            pin.IconPath = IconsPath.SelectedPin;
        //            continue;
        //        }
        //        pin.IconPath = IconsPath.WaypointPin;
        //    }
        //    OnPropertyChanged(nameof(Pins));
        //}

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
