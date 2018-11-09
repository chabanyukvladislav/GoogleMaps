using System.ComponentModel;
using Maps.Collections;
using Maps.Controls;

namespace Maps.ViewModels
{
    public class MapViewModel : IMapViewModel
    {
        private readonly SharedMyPins _myPins;

        public MyPins Pins
        {
            get => _myPins.Pins;
            set
            {
                _myPins.Pins = value;
                OnPropertyChanged(nameof(Pins));
            }
        }

        public MapViewModel()
        {
            _myPins = SharedMyPins.Get;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
