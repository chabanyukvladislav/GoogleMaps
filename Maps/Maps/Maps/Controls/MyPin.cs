using System;
using Maps.Content;
using Maps.Controls.Models;
using MapsApiStandardLibrary.Models.Directions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps.Controls
{
    public class MyPin : Pin
    {
        public static readonly BindableProperty IconPathProperty;
        public static readonly BindableProperty MyTypeProperty;
        public static readonly BindableProperty CoordinateProperty;

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
        public Coordinate Coordinate
        {
            get => (Coordinate)GetValue(CoordinateProperty);
            set => SetValue(CoordinateProperty, value);
        }

        static MyPin()
        {
            IconPathProperty = BindableProperty.Create(nameof(IconPath), typeof(string), typeof(MyPin), IconsPath.WaypointPin);
            MyTypeProperty = BindableProperty.Create(nameof(MyType), typeof(MyPinType), typeof(MyPin), MyPinType.Waypoint);
            CoordinateProperty = BindableProperty.Create(nameof(Coordinate), typeof(Coordinate), typeof(MyPin));
        }

        public MyPin(MyPinType type = MyPinType.Waypoint, string iconPath = IconsPath.WaypointPin, string label = "", Coordinate coordinate = null)
        {
            MyType = type;
            IconPath = iconPath;
            Label = label;
            Coordinate = coordinate;
        }

        public override bool Equals(object value)
        {
            if (!(value is MyPin pin) || pin.Coordinate == null || Coordinate == null)
            {
                return false;
            }

            var coordinateEqual = Coordinate.Equals(pin.Coordinate);
            var typeEqual = MyType == MyPinType.Undefined || pin.MyType == MyPinType.Undefined || MyType == pin.MyType;
            return coordinateEqual && typeEqual;
        }

        public override int GetHashCode()
        {
            return "mp".GetHashCode();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == CoordinateProperty.PropertyName)
            {
                CoordinateChanged?.Invoke(this, Coordinate);
            }
        }

        public event Action<MyPin, Coordinate> CoordinateChanged;
    }
}
