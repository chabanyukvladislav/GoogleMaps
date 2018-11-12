using Maps.Collections;
using Maps.Controls.Models;
using Maps.ViewModels;
using Telerik.XamarinForms.DataControls.ListView;
using Xamarin.Forms;

namespace Maps.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void RadListView_OnReorderEnded(object sender, ReorderEndedEventArgs e)
        {
            var viewModel = (MainViewModel)BindingContext;
            var pins = SharedMyPins.Get;

            pins.UpdatePinsFromPinPoints(viewModel.PinsViewModel.PinPoints);
        }

        private static void ExecuteTypeSelected(object type)
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
        }
    }
}