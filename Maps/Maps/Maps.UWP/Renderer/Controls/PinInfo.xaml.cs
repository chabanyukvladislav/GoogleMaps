using System;
using System.Windows.Input;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms;

namespace Maps.UWP.Renderer.Controls
{
    public sealed partial class PinInfo
    {
        private readonly MapIcon _pin;

        public ICommand Click { get; }

        public PinInfo(MapIcon pin)
        {
            _pin = pin;
            InitializeComponent();
            Click = new Command(OnClick);
        }

        private void OnClick(object obj)
        {
            Clicked?.Invoke(_pin);
        }

        public event Action<MapIcon> Clicked;
    }
}
