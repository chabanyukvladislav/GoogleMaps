using System;
using System.Windows.Input;
using Windows.UI.Xaml.Controls.Maps;
using Maps.Controls.Models;
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
            var pinAction = Enum.Parse<PinAction>(obj.ToString());
            Clicked?.Invoke(_pin, pinAction);
        }

        public event Action<MapIcon, PinAction> Clicked;
    }
}
