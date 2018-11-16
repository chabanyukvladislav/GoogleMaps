using Xamarin.Forms.Xaml;
using Maps.Views;
using Xamarin.Forms;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Maps
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new MainPage());
        }
    }
}
