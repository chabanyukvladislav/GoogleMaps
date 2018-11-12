using System;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Maps.Content;
using Maps.Controls;
using Maps.Controls.Models;
using MapsApiStandardLibrary.Models.Directions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps.UWP.Renderer.Controls
{
    public sealed partial class MapAddPin
    {
        public MyPin Pin { get; }

        public ICommand RadioButtonClick { get; }

        public MapAddPin(BasicGeoposition position)
        {
            InitializeComponent();
            Pin = new MyPin
            {
                Coordinate = new Coordinate(position.Latitude, position.Longitude)
            };
            RadioButtonClick = new Command(OnRadioButtonClick);
        }

        private void OnRadioButtonClick(object obj)
        {
            var type = Enum.Parse<MyPinType>(obj.ToString());
            switch (type)
            {
                case MyPinType.Start:
                    Pin.IconPath = IconsPath.StartEndPin;
                    Pin.Label = MyPinType.Start.ToString();
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
                    Pin.Label = MyPinType.End.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            TypeSelected?.Invoke(Pin);
        }

        public event Action<MyPin> TypeSelected;
    }
}
