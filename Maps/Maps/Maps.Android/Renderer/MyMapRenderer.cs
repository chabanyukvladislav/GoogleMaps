using System.Collections.ObjectModel;
using System.Linq;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Maps.Content;
using Maps.Controls;
using Maps.Controls.Models;
using Maps.Droid.Renderer;
using Maps.Droid.Renderer.Controls;
using Maps.Models.Controls;
using Maps.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(MyMap), typeof(MyMapRenderer))]
namespace Maps.Droid.Renderer
{
    public class MyMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        private ObservableCollection<MyPin> _customPins;

        public MyMapRenderer(Context context) : base(context) { }

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
            var formsMap = (MyMap)e.NewElement;
            _customPins = formsMap.PinsSource;
            Control.GetMapAsync(this);
            foreach (var myPin in _customPins)
            {
                AddMyPin(myPin);
            }
        }

        private void AddMyPin(MyPin myPin)
        {
            var pin = new Pin()
            {
                Label = myPin.Label,
                Position = new Position(myPin.Position.Latitude, myPin.Position.Longitude)
            };
            var marker = CreateMarker(pin);
            var icon = -1;
            switch (myPin.IconPath)
            {
                case IconsPath.StartEndPin:
                    icon = Resource.Drawable.start_end_pin;
                    break;
                case IconsPath.WaypointPin:
                    icon = Resource.Drawable.waypoint_pin;
                    break;
                case IconsPath.MyLocation:
                    icon = Resource.Drawable.my;
                    break;
            }
            marker.SetIcon(BitmapDescriptorFactory.FromResource(icon));
            NativeMap.AddMarker(marker);
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.MapClick += OnMapClicked;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            return marker;
        }

        private static MainViewModel GetCurrentViewModel()
        {
            var view = (Views.MainPage)Application.Current.MainPage;
            var viewModel = (MainViewModel)view.BindingContext;
            return viewModel;
        }

        //private void AddWindow(DependencyObject obj, BasicGeoposition position)
        //{
        //    _nativeMap.Children.Add(obj);
        //    var point = new Geopoint(position);
        //    MapControl.SetLocation(obj, point);
        //    MapControl.SetNormalizedAnchorPoint(obj, new Point(0.5, 1.0));
        //}

        private void OnMapClicked(object sender, GoogleMap.MapClickEventArgs e)
        {
            //if (CloseOldWindows())
            //{
            //    return;
            //}

            var viewModel = GetCurrentViewModel();

            if (viewModel.Pins.Count == 10)
            {
                Application.Current.MainPage.DisplayAlert("Error", "You can`t add more then 10 point", "Ok");
                return;
            }

            var pinAdd = new AddPin(e.Point);
            pinAdd.TypeSelected += OnTypeSelected;
            
            //var position = 
            //AddWindow(pinAdd, position);
        }

        private bool CloseOldWindows()
        {
            //var oldFirstWindow = _nativeMap.Children.FirstOrDefault(el => el is MapAddPin);
            //var oldSecondWindow = _nativeMap.Children.FirstOrDefault(el => el is PinInfo);
            //if (oldSecondWindow == null && oldFirstWindow == null)
            //{
            //    return false;
            //}
            //if (oldFirstWindow != null)
            //{
            //    _nativeMap.Children.Remove(oldFirstWindow);
            //}

            //if (oldSecondWindow != null)
            //{
            //    _nativeMap.Children.Remove(oldSecondWindow);
            //}

            return true;
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

            //var window = NativeMap.Children.FirstOrDefault(el => el is MapAddPin);
            //if (window != null)
            //{
            //    NativeMap.Children.Remove(window);
            //}


            AddMyPin(pin);
            viewModel.Pins.Add(pin);
        }

        public View GetInfoContents(Marker marker)
        {
            //CloseOldWindows();

            //var element = args.MapElements.FirstOrDefault(el => el is MapIcon);
            //if (element == null)
            //{
            //    return;
            //}

            //var pinInfo = new PinInfo(element as MapIcon);
            //pinInfo.Clicked += ActionSelected;
            //var position = new BasicGeoposition
            //{
            //    Latitude = args.Location.Position.Latitude,
            //    Longitude = args.Location.Position.Longitude
            //};
            //AddWindow(pinInfo, position);
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            return null;
        }
    }
}
