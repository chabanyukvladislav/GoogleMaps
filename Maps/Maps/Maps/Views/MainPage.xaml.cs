using Maps.Collections;
using Maps.ViewModels;
using Telerik.XamarinForms.DataControls.ListView;

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
    }
}