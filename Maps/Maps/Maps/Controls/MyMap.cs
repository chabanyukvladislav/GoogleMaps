using System;
using System.Collections.ObjectModel;
using Maps.Content;
using Maps.Controls.Models;
using Maps.Models.Controls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Location = MapsApiLibrary.Api.Parameters.Directions.Location;

namespace Maps.Controls
{
    public class MyMap : Map
    {
        public static readonly BindableProperty CurrentLocationProperty;
        public static readonly BindableProperty RadiusProperty;
        public static readonly BindableProperty PinsSourceProperty;

        public Location CurrentLocation
        {
            get => (Location)GetValue(CurrentLocationProperty);
            set => SetValue(CurrentLocationProperty, value);
        }
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }
        public ObservableCollection<MyPin> PinsSource
        {
            get => (ObservableCollection<MyPin>)GetValue(PinsSourceProperty);
            set => SetValue(PinsSourceProperty, value);
        }

        static MyMap()
        {
            CurrentLocationProperty = BindableProperty.Create(nameof(CurrentLocation), typeof(Location), typeof(MyMap), new Location(), BindingMode.TwoWay, CurrentLocationValidate);
            RadiusProperty = BindableProperty.Create(nameof(Radius), typeof(double), typeof(MyMap), (double)2000, BindingMode.TwoWay, RadiusValidate);
            PinsSourceProperty = BindableProperty.Create(nameof(PinsSource), typeof(ObservableCollection<MyPin>), typeof(MyMap), new ObservableCollection<MyPin>(), BindingMode.TwoWay, PinsSourceValidate);
        }

        public MyMap()
        {
            GetCurrentLocation();
        }

        private async void GetCurrentLocation()
        {
            CurrentLocation = new Location(46.440033, 30.756811);
            try
            {
                var position = await Geolocation.GetLocationAsync();
                var myLocation = new Location(position.Latitude, position.Longitude);
                CurrentLocation = myLocation;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void MapMove()
        {
            if (CurrentLocation.Latitude != null && CurrentLocation.Longitude != null)
            {
                MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(CurrentLocation.Latitude.Value, CurrentLocation.Longitude.Value),
                    new Distance(Radius)));
            }
        }

        private static bool CurrentLocationValidate(BindableObject bindable, object value)
        {
            var location = (Location)value;
            return location?.Longitude != null && location.Latitude != null;
        }
        private static bool RadiusValidate(BindableObject bindable, object value)
        {
            var radius = (double)value;
            return radius > 0;
        }

        private static bool PinsSourceValidate(BindableObject bindable, object value)
        {
            var pinsSource = (ObservableCollection<MyPin>)value;
            return pinsSource != null;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == CurrentLocationProperty.PropertyName || propertyName == nameof(Location.Latitude) || propertyName == nameof(Location.Longitude))
            {
                MapMove();
            }
        }
    }
}