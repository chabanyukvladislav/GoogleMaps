using Maps.Content;
using Maps.Controls.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps.Models.Controls
{
    public class MyPin : Pin
    {
        public static readonly BindableProperty IconPathProperty;
        public static readonly BindableProperty MyTypeProperty;

        public string IconPath
        {
            get => (string)GetValue(IconPathProperty);
            set => SetValue(IconPathProperty, value);
        }
        public MyPinType MyType
        {
            get => (MyPinType)GetValue(MyTypeProperty);
            set => SetValue(MyTypeProperty, value);
        }

        static MyPin()
        {
            IconPathProperty = BindableProperty.Create(nameof(IconPath), typeof(string), typeof(MyPin));
            MyTypeProperty = BindableProperty.Create(nameof(MyType), typeof(MyPinType), typeof(MyPin));
        }

        public MyPin()
        {
            IconPath = IconsPath.WaypointPin;
            Label = "";
            Position = new Position();
            MyType = MyPinType.Waypoint;
        }
    }
}
