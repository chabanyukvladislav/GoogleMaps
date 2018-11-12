using System;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using Maps.Collections;
using Maps.Controls;
using Maps.Controls.Models;
using Maps.UWP.Renderer;
using Maps.UWP.Renderer.Controls;
using Maps.UWP.Renderer.Helpers;
using MapsApiStandardLibrary.Models.Directions;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;
using Application = Xamarin.Forms.Application;
using Point = Windows.Foundation.Point;

[assembly: ExportRenderer(typeof(MyMap), typeof(MyMapRenderer))]
namespace Maps.UWP.Renderer
{
    public class MyMapRenderer : MapRenderer
    {
        private readonly SharedMyPins _pins;
        private readonly SharedRoutePath _routePath;
        private MapControl _nativeMap;
        private bool _iconClicked;

        public MyMapRenderer()
        {
            _pins = SharedMyPins.Get;
            _routePath = SharedRoutePath.Get;

            _pins.PinAdded += OnAddPin;
            _pins.PinRemoved += OnRemovePin;
            _pins.PinSelected += OnPinSelected;
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

        private void AddWindow(DependencyObject obj, BasicGeoposition position)
        {
            _nativeMap.Children.Add(obj);
            var point = new Geopoint(position);
            MapControl.SetLocation(obj, point);
            MapControl.SetNormalizedAnchorPoint(obj, new Point(0.5, 1.0));
        }
        private bool CloseWindows()
        {
            var oldFirstWindow = _nativeMap.Children.FirstOrDefault(el => el is MapAddPin);
            var oldSecondWindow = _nativeMap.Children.FirstOrDefault(el => el is PinInfo);
            if (oldSecondWindow == null && oldFirstWindow == null)
            {
                return false;
            }
            if (oldFirstWindow != null)
            {
                _nativeMap.Children.Remove(oldFirstWindow);
            }

            if (oldSecondWindow != null)
            {
                _nativeMap.Children.Remove(oldSecondWindow);
            }

            return true;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _nativeMap.Children.Clear();
                _nativeMap.MapTapped -= OnMapClicked;
                _nativeMap.MapElementClick -= OnMapElementClick;
                _nativeMap = null;
            }

            if (e.NewElement == null)
            {
                return;
            }

            _nativeMap = Control;
            _nativeMap.MapTapped += OnMapClicked;
            _nativeMap.MapElementClick += OnMapElementClick;
            _nativeMap.MapElements.Clear();
            _nativeMap.Children.Clear();

            LoadMyPins();
        }

        private void OnMapClicked(MapControl sender, MapInputEventArgs args)
        {
            if (_iconClicked)
            {
                _iconClicked = false;
                return;
            }

            if (CloseWindows())
            {
                return;
            }

            if (_pins.Pins.WaypointsPin.Count == 8)
            {
                Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 8 waypoints", "Ok");
                return;
            }

            var pinAdd = new MapAddPin(args.Location.Position);
            pinAdd.TypeSelected += OnTypeSelected;
            var position = new BasicGeoposition
            {
                Latitude = args.Location.Position.Latitude,
                Longitude = args.Location.Position.Longitude
            };
            AddWindow(pinAdd, position);
        }
        private void OnTypeSelected(MyPin pin)
        {
            switch (pin.MyType)
            {
                case MyPinType.Start when _pins.Pins.StartPin.Coordinate != null:
                    Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 1 start point", "Ok");
                    return;
                case MyPinType.End when _pins.Pins.EndPin.Coordinate != null:
                    Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 1 end point", "Ok");
                    return;
            }

            _pins.AddPin(pin);

            var window = _nativeMap.Children.FirstOrDefault(el => el is MapAddPin);
            if (window != null)
            {
                _nativeMap.Children.Remove(window);
            }
        }

        private void OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            CloseWindows();

            var element = args.MapElements.FirstOrDefault(el => el is MapIcon icon && icon.Title != "I");
            if (element == null)
            {
                return;
            }

            var pinInfo = new PinInfo(element as MapIcon);
            pinInfo.Clicked += OnActionSelected;
            var position = new BasicGeoposition
            {
                Latitude = args.Location.Position.Latitude,
                Longitude = args.Location.Position.Longitude
            };
            AddWindow(pinInfo, position);

            _iconClicked = true;
        }
        private void OnActionSelected(MapIcon pin, PinAction action)
        {
            switch (action)
            {
                case PinAction.Delete:
                    var myPin = new MyPin
                    {
                        Coordinate = new Coordinate(pin.Location.Position.Latitude, pin.Location.Position.Longitude),
                        MyType = MyPinType.Undefined
                    };
                    _pins.RemovePin(myPin);

                    var window = _nativeMap.Children.FirstOrDefault(el => el is PinInfo);
                    if (window != null)
                    {
                        _nativeMap.Children.Remove(window);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnRenderPath()
        {
            var element = _nativeMap.MapElements.FirstOrDefault(el => el is MapPolyline);
            if (element != null)
            {
                _nativeMap.MapElements.Remove(element);
            }

            var positions = LocationsToBasicGeopositions.Convert(_routePath.Coordinates);
            var polyline = new MapPolyline
            {
                StrokeColor = Colors.DarkRed,
                StrokeThickness = 5,
                Path = new Geopath(positions)
            };
            _nativeMap.MapElements.Add(polyline);
        }

        private void OnAddPin(MyPin pin)
        {
            var pinPosition = new BasicGeoposition
            {
                Latitude = pin.Coordinate.Latitude,
                Longitude = pin.Coordinate.Longitude
            };
            var pinPoint = new Geopoint(pinPosition);
            var mapIcon = new MapIcon
            {
                Image = RandomAccessStreamReference.CreateFromUri(new Uri($"ms-appx:///{pin.IconPath}")),
                CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible,
                Location = pinPoint,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = pin.Label
            };
            _nativeMap.MapElements.Add(mapIcon);
        }
        private void OnRemovePin(Coordinate pinCoordinate)
        {
            var element = _nativeMap.MapElements.FirstOrDefault(el =>
                el is MapIcon icon &&
                new Coordinate(icon.Location.Position.Latitude, icon.Location.Position.Longitude)
                    .Equals(pinCoordinate));
            if (element != null)
            {
                _nativeMap.MapElements.Remove(element);
            }
        }
        private void OnPinSelected(MyPin newPin, MyPin oldPin)
        {
            if(oldPin != null)
            {
                OnRemovePin(oldPin.Coordinate);
                OnAddPin(oldPin);
            }

            if (newPin == null)
            {
                return;
            }
            OnRemovePin(newPin.Coordinate);
            OnAddPin(newPin);
        }
        private void OnPinsUpdated()
        {
            _nativeMap.MapElements.Clear();
            LoadMyPins();
        }
    }
}