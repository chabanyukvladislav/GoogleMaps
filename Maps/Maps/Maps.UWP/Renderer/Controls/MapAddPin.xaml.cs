using System;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Maps.Content;
using Maps.Controls.Models;
using Maps.Models.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps.UWP.Renderer.Controls
{
    public sealed partial class MapAddPin
    {
        public ICommand RadioButtonClick { get; }
        public MyPin Pin { get; }

        public MapAddPin(BasicGeoposition position)
        {
            InitializeComponent();
            RadioButtonClick = new Command(OnRadioButtonClick);
            Pin = new MyPin
            {
                Position = new Position(position.Latitude, position.Longitude)
            };
        }

        private void OnRadioButtonClick(object obj)
        {
            var typeString = obj.ToString();
            var type = Enum.Parse<MyPinType>(typeString);
            switch (type)
            {
                case MyPinType.Start:
                    Pin.IconPath = IconsPath.StartEndPin;
                    Pin.Label = "Start";
                    Pin.MyType = MyPinType.Start;
                    break;
                case MyPinType.Waypoint:
                    Pin.IconPath = IconsPath.WaypointPin;
                    Pin.MyType = MyPinType.Waypoint;
                    Pin.Label = "";
                    break;
                case MyPinType.End:
                    Pin.IconPath = IconsPath.StartEndPin;
                    Pin.MyType = MyPinType.End;
                    Pin.Label = "End";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            TypeSelected?.Invoke(Pin);
        }

        public event Action<MyPin> TypeSelected;
    }
}
