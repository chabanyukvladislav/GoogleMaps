using System.Linq;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Maps.Collections;
using Maps.Content;
using Maps.Controls;
using Maps.Controls.Models;
using Maps.Droid.Renderer;
using Maps.Droid.Renderer.Helpers;
using MapsApiStandardLibrary.Models.Directions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyMap), typeof(MyMapRenderer))]
namespace Maps.Droid.Renderer
{
    public class MyMapRenderer : MapRenderer
    {
        private readonly SharedMyPins _pins;
        private readonly SharedRoutePath _routePath;

        public MyMapRenderer(Context context) : base(context)
        {
            _pins = SharedMyPins.Get;
            _routePath = SharedRoutePath.Get;

            _pins.PinAdded += OnAddPin;
            _pins.PinRemoved += x => OnPinsUpdated();
            _pins.PinSelected += (x, y) => OnPinsUpdated();
            _pins.PinsUpdated += OnPinsUpdated;
            _routePath.CoordinatesChanged += OnRenderPath;
        }

        private void LoadMyPins()
        {
            if (_pins.Pins.MyCoordinatePin.Coordinate != null)
            {
                OnAddPin(_pins.Pins.MyCoordinatePin);
            }
            if (_pins.Pins.StartPin.Coordinate != null)
            {
                OnAddPin(_pins.Pins.StartPin);
            }
            foreach (var myPin in _pins.Pins.WaypointsPin)
            {
                OnAddPin(myPin);
            }
            if (_pins.Pins.EndPin.Coordinate != null)
            {
                OnAddPin(_pins.Pins.EndPin);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.MapClick -= OnMapClicked;
            }

            if (e.NewElement == null)
            {
                return;
            }

            Control.GetMapAsync(this);
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.MapClick += OnMapClicked;
            LoadMyPins();
        }
        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var myPin = (MyPin)pin;
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(myPin.Coordinate.Latitude, myPin.Coordinate.Longitude));
            marker.SetTitle(myPin.Label);
            var id = Resources.GetIdentifier(myPin.IconPath.Remove(myPin.IconPath.IndexOf('.')), "drawable", Context.PackageName);
            marker.SetIcon(BitmapDescriptorFactory.FromResource(id));
            return marker;
        }

        private void OnMapClicked(object sender, GoogleMap.MapClickEventArgs args)
        {
            if (_pins.Pins.WaypointsPin.Count == 8)
            {
                Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 8 waypoints", "Ok");
                return;
            }

            var coordinate = new Coordinate(args.Point.Latitude, args.Point.Longitude);
            var type = _pins.PinType;
            var label = "";
            var path = "";
            switch (type)
            {
                case MyPinType.Start:
                    if (_pins.Pins.StartPin.Coordinate != null)
                    {
                        Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 1 start point",
                            "Ok");
                        return;
                    }
                    label = "Start";
                    path = IconsPath.StartPin;
                    break;
                case MyPinType.Waypoint:
                    label = "";
                    path = IconsPath.WaypointPin;
                    break;
                case MyPinType.End:
                    if (_pins.Pins.EndPin.Coordinate != null)
                    {
                        Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 1 end point",
                            "Ok");
                        return;
                    }
                    label = "End";
                    path = IconsPath.EndPin;
                    break;
                case MyPinType.Undefined:
                    Application.Current.MainPage.DisplayAlert("Error", "You should check one type of pin", "Ok");
                    return;
            }

            var pin = new MyPin(type, path, label, coordinate);
            _pins.AddPin(pin);
        }

        private void OnRenderPath()
        {
            OnPinsUpdated();

            var polyline = new PolylineOptions().Add(LocationsToPoints.Convert(_routePath.Coordinates).ToArray())
                .InvokeColor(Android.Graphics.Color.DarkRed);
            NativeMap.AddPolyline(polyline);
        }

        private void OnAddPin(MyPin pin)
        {
            var marker = CreateMarker(pin);
            NativeMap.AddMarker(marker);
        }
        private void OnPinsUpdated()
        {
            NativeMap.Clear();
            LoadMyPins();
        }
    }
}