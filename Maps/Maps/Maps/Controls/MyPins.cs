using System;
using System.Collections.ObjectModel;
using System.Linq;
using Maps.Content;
using Maps.Controls.Models;
using Xamarin.Forms;

namespace Maps.Controls
{
    public class MyPins : BindableObject
    {
        public static readonly BindableProperty MyCoordinatePinProperty;
        public static readonly BindableProperty StartPinProperty;
        public static readonly BindableProperty WaypointsPinProperty;
        public static readonly BindableProperty EndPinProperty;

        public MyPin MyCoordinatePin
        {
            get => (MyPin)GetValue(MyCoordinatePinProperty);
            set => SetValue(MyCoordinatePinProperty, value);
        }
        public MyPin StartPin
        {
            get => (MyPin)GetValue(StartPinProperty);
            set => SetValue(StartPinProperty, value);
        }
        public ObservableCollection<MyPin> WaypointsPin
        {
            get => (ObservableCollection<MyPin>)GetValue(WaypointsPinProperty);
            set => SetValue(WaypointsPinProperty, value);
        }
        public MyPin EndPin
        {
            get => (MyPin)GetValue(EndPinProperty);
            set => SetValue(EndPinProperty, value);
        }

        static MyPins()
        {
            MyCoordinatePinProperty = BindableProperty.Create(nameof(MyCoordinatePin), typeof(MyPin), typeof(MyPins),
                new MyPin(MyPinType.MyLocation, IconsPath.MyLocation, "I"), BindingMode.TwoWay, NotNullValidate);
            StartPinProperty = BindableProperty.Create(nameof(StartPin), typeof(MyPin), typeof(MyPins),
                new MyPin(MyPinType.Start, IconsPath.StartEndPin, MyPinType.Start.ToString()), BindingMode.TwoWay,
                NotNullValidate);
            WaypointsPinProperty = BindableProperty.Create(nameof(WaypointsPin), typeof(ObservableCollection<MyPin>),
                typeof(MyPins), new ObservableCollection<MyPin>(), BindingMode.TwoWay, NotNullValidate);
            EndPinProperty = BindableProperty.Create(nameof(EndPin), typeof(MyPin), typeof(MyPins),
                new MyPin(MyPinType.End, IconsPath.StartEndPin, MyPinType.End.ToString()), BindingMode.TwoWay,
                NotNullValidate);
        }

        public void Add(MyPin pin)
        {
            switch (pin.MyType)
            {
                case MyPinType.Start:
                    StartPin.Coordinate = pin.Coordinate;
                    break;
                case MyPinType.Waypoint:
                    if (WaypointsPin.FirstOrDefault(el => el.Coordinate.Equals(pin.Coordinate)) != null)
                    {
                        return;
                    }
                    WaypointsPin.Add(pin);
                    break;
                case MyPinType.End:
                    EndPin.Coordinate = pin.Coordinate;
                    break;
                case MyPinType.MyLocation:
                    MyCoordinatePin.Coordinate = pin.Coordinate;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PinAdded?.Invoke(pin);
        }
        public void Remove(MyPin pin)
        {
            switch (pin.MyType)
            {
                case MyPinType.Start:
                    StartPin.Coordinate = null;
                    break;
                case MyPinType.Waypoint:
                    var element = WaypointsPin.FirstOrDefault(el => el.Coordinate.Equals(pin.Coordinate));
                    if (element != null)
                    {
                        WaypointsPin.Remove(element);
                    }
                    break;
                case MyPinType.End:
                    EndPin.Coordinate = null;
                    break;
                case MyPinType.MyLocation:
                    MyCoordinatePin.Coordinate = null;
                    break;
                default:
                    var item = WaypointsPin.FirstOrDefault(el => el.Coordinate.Equals(pin.Coordinate));
                    if (item != null)
                    {
                        WaypointsPin.Remove(item);
                        break;
                    }

                    if (StartPin.Coordinate != null && StartPin.Coordinate.Equals(pin.Coordinate))
                    {
                        StartPin.Coordinate = null;
                        break;
                    }

                    if (EndPin.Coordinate != null && EndPin.Coordinate.Equals(pin.Coordinate))
                    {
                        EndPin.Coordinate = null;
                        break;
                    }

                    if (MyCoordinatePin.Coordinate != null && MyCoordinatePin.Coordinate.Equals(pin.Coordinate))
                    {
                        MyCoordinatePin.Coordinate = null;
                    }
                    break;
            }

            PinRemoved?.Invoke(pin);
        }

        private static bool NotNullValidate(BindableObject bindable, object value)
        {
            return value != null;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
        
        public event Action<MyPin> PinAdded;
        public event Action<MyPin> PinRemoved;
    }
}
