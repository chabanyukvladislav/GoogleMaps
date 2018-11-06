using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using Maps.Controls;
using Maps.Controls.Models;
using Maps.Helpers;
using Maps.Models.Controls;
using Maps.UWP.Renderer;
using Maps.UWP.Renderer.Controls;
using Maps.UWP.Renderer.Helpers;
using Maps.ViewModels;
using Xamarin.Forms;
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
        private MapControl _nativeMap;
        private ObservableCollection<MyPin> _myPins;
        private bool _iconClicked;

        private void AddMyPin(MyPin pin)
        {
            var pinPosition = new BasicGeoposition
            {
                Latitude = pin.Position.Latitude,
                Longitude = pin.Position.Longitude
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

        private static MainViewModel GetCurrentViewModel()
        {
            var view = ((NavigationPage)Application.Current.MainPage).Pages.First();
            var viewModel = (MainViewModel)view.BindingContext;
            return viewModel;
        }//????????

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            var viewModel = GetCurrentViewModel();
                viewModel.PathRender += OnRenderPath;
            viewModel.PropertyChanged += OnUpdate;

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

            var formsMap = (MyMap)e.NewElement;
            _nativeMap = Control;
            _nativeMap.MapTapped += OnMapClicked;
            _nativeMap.MapElementClick += OnMapElementClick;

            _myPins = formsMap.PinsSource;
            _nativeMap.Children.Clear();
            foreach (var pin in _myPins)
            {
                AddMyPin(pin);
            }
        }

        private void OnUpdate(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Pins")
            {
                return;
            }

            foreach (var element in _nativeMap.MapElements.Where(el => el is MapIcon).ToList())
            {
                _nativeMap.MapElements.Remove(element);
            }
            foreach (var pin in ((MainViewModel)sender).Pins)
            {
                AddMyPin(pin);
            }
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

            var viewModel = GetCurrentViewModel();

            if (viewModel.Pins.Count == 10)
            {
                Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 10 point", "Ok");
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
            var viewModel = GetCurrentViewModel();

            if (pin.MyType == MyPinType.Start && viewModel.Pins.Any(el => el.MyType == MyPinType.Start))
            {
                Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 1 start point", "Ok");
                return;
            }
            else if (pin.MyType == MyPinType.End && viewModel.Pins.Any(el => el.MyType == MyPinType.End))
            {
                Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 1 end point", "Ok");
                return;
            }

            var window = _nativeMap.Children.FirstOrDefault(el => el is MapAddPin);
            if (window != null)
            {
                _nativeMap.Children.Remove(window);
            }
            
            AddMyPin(pin);
            viewModel.Pins.Add(pin);
        }

        private void OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            CloseOldWindows();

            var element = args.MapElements.FirstOrDefault(el => el is MapIcon);
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

        private void OnActionSelected(MapIcon pin)
        {
            _nativeMap.MapElements.Remove(pin);

            var viewModel = GetCurrentViewModel();
            var element = viewModel.Pins.FirstOrDefault(el =>
                Math.Abs(el.Position.Latitude - pin.Location.Position.Latitude) <= 0 &&
                Math.Abs(el.Position.Longitude - pin.Location.Position.Longitude) <= 0);
            viewModel.Pins.Remove(element);

            var window = _nativeMap.Children.FirstOrDefault(el => el is PinInfo);
            if (window != null)
            {
                _nativeMap.Children.Remove(window);
            }
        }

        private void OnRenderPath(string obj)
        {
            var element = _nativeMap.MapElements.FirstOrDefault(el => el is MapPolyline);
            if (element != null)
            {
                _nativeMap.MapElements.Remove(element);
            }

            var points = PolylineDecoder.Decode(obj);
            var positions = LocationsToBasicGeopositions.Convert(points);
            var polyline = new MapPolyline
            {
                StrokeColor = Colors.Red,
                StrokeThickness = 5,
                Path = new Geopath(positions)
            };
            _nativeMap.MapElements.Add(polyline);
        }
    }
}
