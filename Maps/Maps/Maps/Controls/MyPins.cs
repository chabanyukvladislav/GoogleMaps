using System.Collections.ObjectModel;
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

        private static bool NotNullValidate(BindableObject bindable, object value)
        {
            return value != null;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
