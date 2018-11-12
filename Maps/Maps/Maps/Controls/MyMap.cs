using System;
using MapsApiStandardLibrary.Models.Directions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps.Controls
{
    public class MyMap : Map
    {
        public static readonly BindableProperty CurrentCoordinateProperty;
        public static readonly BindableProperty RadiusProperty;
        public static readonly BindableProperty PinsSourceProperty;

        public Coordinate CurrentCoordinate
        {
            get => (Coordinate)GetValue(CurrentCoordinateProperty);
            set => SetValue(CurrentCoordinateProperty, value);
        }
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }
        public MyPins PinsSource
        {
            get => (MyPins)GetValue(PinsSourceProperty);
            set => SetValue(PinsSourceProperty, value);
        }

        static MyMap()
        {
            CurrentCoordinateProperty = BindableProperty.Create(nameof(CurrentCoordinate), typeof(Coordinate), typeof(MyMap), new Coordinate(), BindingMode.TwoWay, ValueNotNullValidate);
            RadiusProperty = BindableProperty.Create(nameof(Radius), typeof(double), typeof(MyMap), (double)2000, BindingMode.TwoWay, RadiusValidate);
            PinsSourceProperty = BindableProperty.Create(nameof(PinsSource), typeof(MyPins), typeof(MyMap), new MyPins(), BindingMode.TwoWay, ValueNotNullValidate);
        }

        public MyMap()
        {
            GetCurrentLocation();
        }

        private async void GetCurrentLocation()
        {
            Coordinate myLocation;
            try
            {
                var position = await Geolocation.GetLocationAsync();
                myLocation = new Coordinate(position.Latitude, position.Longitude);
            }
            catch (Exception)
            {
                myLocation = new Coordinate(46.440033, 30.756811);
            }
            CurrentCoordinate = myLocation;
        }

        private void MapMove()
        {
            MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(CurrentCoordinate.Latitude, CurrentCoordinate.Longitude),
                new Distance(Radius)));
        }

        private static bool RadiusValidate(BindableObject bindable, object value)
        {
            var radius = (double)value;
            return radius > 0;
        }
        private static bool ValueNotNullValidate(BindableObject bindable, object value)
        {
            return value != null;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == CurrentCoordinateProperty.PropertyName || propertyName == nameof(Coordinate.Latitude) ||
                propertyName == nameof(Coordinate.Longitude) || propertyName == RadiusProperty.PropertyName)
            {
                MapMove();
            }
        }
    }
}