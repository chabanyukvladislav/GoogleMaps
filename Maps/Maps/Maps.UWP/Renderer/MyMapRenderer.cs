using System;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Maps.Content;
using Maps.Controls;
using Maps.Models.Controls;
using Maps.UWP.Renderer;
using Maps.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;
using Point = Windows.Foundation.Point;

[assembly: ExportRenderer(typeof(MyMap), typeof(MyMapRenderer))]
namespace Maps.UWP.Renderer
{
    public class MyMapRenderer : MapRenderer
    {
        private MapControl _nativeMap;
        private ObservableCollection<MyPin> _myPins;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _nativeMap.Children.Clear();
                _nativeMap = null;
            }

            if (e.NewElement == null)
            {
                return;
            }

            var formsMap = (MyMap)e.NewElement;
            _nativeMap = Control;
            _nativeMap.MapTapped += OnPinCreated;
            _myPins = formsMap.PinsSource;
            _nativeMap.Children.Clear();
            foreach (var pin in _myPins)
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
        }

        private void OnPinCreated(MapControl sender, MapInputEventArgs args)
        {
            var view = (Views.MainPage)Application.Current.MainPage;
            var viewModel = (MainViewModel)view.BindingContext;
            if (viewModel.Pins.Count == 10)
            {
                Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 10 point", "Ok");
                return;
            }
            var pin = new MyPin
            {
                IconPath = IconsPath.UserPin,
                Position = new Position(args.Location.Position.Latitude, args.Location.Position.Longitude),
                Label = (viewModel.Pins.Count == 0 ? "Start" : (viewModel.Pins.Count == 9 ? "End" : ""))
            };
            var mapIcon = new MapIcon
            {
                Image = RandomAccessStreamReference.CreateFromUri(new Uri($"ms-appx:///{pin.IconPath}")),
                CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible,
                Location = new Geopoint(new BasicGeoposition
                {
                    Latitude = pin.Position.Latitude,
                    Longitude = pin.Position.Longitude
                }),
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = pin.Label
            };
            _nativeMap.MapElements.Add(mapIcon);
            viewModel.Pins.Add(pin);
        }

        //private void OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        //{

        //}
    }
}
