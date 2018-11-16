using System;
using System.Collections.ObjectModel;
using Maps.Collections;
using Maps.Controls;
using Maps.Controls.Models;
using Maps.ViewModels;
using Telerik.XamarinForms.DataControls;
using Telerik.XamarinForms.DataControls.ListView;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;

namespace Maps.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            SharedRoutePath.Get.CoordinatesChanged += icon.Activate;
            SharedMyPins.Get.PinSelected += (x, y) => icon.Activate();
        }

        private void RadListView_OnReorderEnded(object sender, ReorderEndedEventArgs e)
        {
            var viewModel = (MainViewModel)BindingContext;
            var pins = SharedMyPins.Get;

            pins.UpdatePinsFromPinPoints(viewModel.PinsViewModel.PinPoints);
        }

        private void ExecuteTypeSelected(object type)
        {
            var myPins = SharedMyPins.Get;
            var pinType = type.ToString();
            switch (pinType)
            {
                case "Start":
                    myPins.PinType = MyPinType.Start;
                    break;
                case "Waypoint":
                    myPins.PinType = MyPinType.Waypoint;
                    break;
                case "End":
                    myPins.PinType = MyPinType.End;
                    break;
            }
        }

        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == false)
            {
                var el = (Switch)sender;
                el.Toggled -= Switch_OnToggled;
                el.IsToggled = true;
                el.Toggled += Switch_OnToggled;
                return;
            }

            var start = (Switch)FindByName("Start");
            var waypoint = (Switch)FindByName("Waypoint");
            var end = (Switch)FindByName("End");

            if (sender.Equals(start))
            {
                waypoint.Toggled -= Switch_OnToggled;
                end.Toggled -= Switch_OnToggled;
                waypoint.IsToggled = false;
                end.IsToggled = false;
                waypoint.Toggled += Switch_OnToggled;
                end.Toggled += Switch_OnToggled;
                ExecuteTypeSelected("Start");
            }
            else if (sender.Equals(waypoint))
            {
                start.Toggled -= Switch_OnToggled;
                end.Toggled -= Switch_OnToggled;
                start.IsToggled = false;
                end.IsToggled = false;
                start.Toggled += Switch_OnToggled;
                end.Toggled += Switch_OnToggled;
                ExecuteTypeSelected("Waypoint");
            }
            else if (sender.Equals(end))
            {
                waypoint.Toggled -= Switch_OnToggled;
                start.Toggled -= Switch_OnToggled;
                start.IsToggled = false;
                waypoint.IsToggled = false;
                waypoint.Toggled += Switch_OnToggled;
                start.Toggled += Switch_OnToggled;
                ExecuteTypeSelected("End");
            }
            icon.Activate();
        }

        private void OnItemSwipeCompleted(object sender, ItemSwipeCompletedEventArgs e)
        {
            var item = e.Item as PinPoint;

            if (!(sender is RadListView listView))
            {
                return;
            }

            listView.EndItemSwipe();

            if (!(e.Offset <= -70) && !(e.Offset >= 70))
            {
                return;
            }
            (listView.ItemsSource as ObservableCollection<PinPoint>)?.Remove(item);
            var pins = SharedMyPins.Get;
            if (item == null)
            {
                return;
            }
            var pin = new MyPin
            {
                MyType = MyPinType.Undefined,
                Coordinate = item.Coordinate
            };
            pins.RemovePin(pin);
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var menu = (RadSideDrawer)Content;
            if (menu.IsOpen)
            {
                menu.IsOpen = false;
                ((MainViewModel)BindingContext).Icon = "down.png";
            }
            else
            {
                menu.IsOpen = true;
                ((MainViewModel)BindingContext).Icon = "up.png";
            }
        }
    }
}