using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Maps.Content;
using Maps.Models.Controls;
using Maps.UWP.Annotations;
using Xamarin.Forms.Maps;

namespace Maps.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<MyPin> _pins;

        public ObservableCollection<MyPin> Pins
        {
            get => _pins;
            private set
            {
                _pins = value;
                OnPropertyChanged(nameof(Pins));
            }
        }

        public MainViewModel()
        {
            var p = new MyPin
            {
                Position = new Position(46.438435, 30.757254),
                Label = "Start",
                IconPath = IconsPath.UserPin
            };
            Pins = new ObservableCollection<MyPin> {p};
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
