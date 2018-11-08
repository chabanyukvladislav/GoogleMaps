﻿using System;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using Maps.Controls;
using Maps.Controls.Models;
using Maps.Helpers;
using Maps.UWP.Renderer;
using Maps.UWP.Renderer.Controls;
using Maps.UWP.Renderer.Helpers;
using Maps.ViewModels;
using MapsApiLibrary.Models.Directions;
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
        private MyPins _pins;
        private MapControl _nativeMap;
        private bool _iconClicked;

        public MyMapRenderer()
        {
            var viewModel = GetMapViewModel();
            viewModel.PathRendering += OnRenderPath;
        }

        private void AddMyPins(MyPins pins)
        {
            _pins = new MyPins();

            if (pins.MyCoordinatePin.Coordinate != null)
            {
                AddMyPin(pins.MyCoordinatePin);
            }
            if (pins.StartPin.Coordinate != null)
            {
                AddMyPin(pins.StartPin);
            }
            foreach (var myPin in pins.WaypointsPin)
            {
                AddMyPin(myPin);
            }
            if (pins.EndPin.Coordinate != null)
            {
                AddMyPin(pins.EndPin);
            }

            _pins.PinAdded += AddPin;
            _pins.PinRemoved += RemovePin;
        }
        private void AddMyPin(MyPin pin)
        {
            _pins.Add(pin);
        }

        private void AddPin(MyPin pin)
        {
            var viewModel = GetMapViewModel();

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
            viewModel.PinsUpdate(_pins);
        }
        private void RemovePin(MyPin pin)
        {
            var viewModel = GetMapViewModel();

            var element = _nativeMap.MapElements.FirstOrDefault(el => el is MapIcon icon &&
                                                                      new MyPin
                                                                      {
                                                                          Coordinate = new Coordinate(icon.Location.Position.Latitude,
                                                                              icon.Location.Position.Longitude)
                                                                      }.Equals(pin));
            if (element != null)
            {
                _nativeMap.MapElements.Remove(element);
            }
            viewModel.PinsUpdate(_pins);
        }

        private void AddWindow(DependencyObject obj, BasicGeoposition position)
        {
            _nativeMap.Children.Add(obj);
            var point = new Geopoint(position);
            MapControl.SetLocation(obj, point);
            MapControl.SetNormalizedAnchorPoint(obj, new Point(0.5, 1.0));
        }
        private bool CloseOldWindows()
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

        private static IMapViewModel GetMapViewModel()
        {
            var view = Application.Current.MainPage;
            var viewModel = (MainViewModel)view.BindingContext;
            return viewModel.MapViewModel;
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

            var formsMap = (MyMap)e.NewElement;
            AddMyPins(formsMap.PinsSource);
        }

        private void OnMapClicked(MapControl sender, MapInputEventArgs args)
        {
            if (_iconClicked)
            {
                _iconClicked = false;
                return;
            }

            if (CloseOldWindows())
            {
                return;
            }

            var viewModel = GetMapViewModel();
            if (viewModel.Pins.WaypointsPin.Count == 8)
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
            var viewModel = GetMapViewModel();

            switch (pin.MyType)
            {
                case MyPinType.Start when viewModel.Pins.StartPin.Coordinate != null:
                    Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 1 start point", "Ok");
                    return;
                case MyPinType.End when viewModel.Pins.EndPin.Coordinate != null:
                    Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 1 end point", "Ok");
                    return;
            }
            
            AddMyPin(pin);

            var window = _nativeMap.Children.FirstOrDefault(el => el is MapAddPin);
            if (window != null)
            {
                _nativeMap.Children.Remove(window);
            }
        }

        private void OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            CloseOldWindows();

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
                    _pins.Remove(myPin);

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

        private void OnRenderPath(string encodePolyline)
        {
            var element = _nativeMap.MapElements.FirstOrDefault(el => el is MapPolyline);
            if (element != null)
            {
                _nativeMap.MapElements.Remove(element);
            }

            var points = PolylineDecoder.Decode(encodePolyline);
            var positions = LocationsToBasicGeopositions.Convert(points);
            var polyline = new MapPolyline
            {
                StrokeColor = Colors.DarkRed,
                StrokeThickness = 5,
                Path = new Geopath(positions)
            };
            _nativeMap.MapElements.Add(polyline);
        }

        private void OnIconUpdate(MyPin pin, string path)
        {
            RemovePin(pin);
            AddMyPin(pin);
        }
    }
}
//Bug with deleting, turn off selecting(not work)