using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps.Models.Controls
{
    public class MyPin : Pin
    {
        public static readonly BindableProperty IconPathProperty;
        
        public string IconPath
        {
            get => (string)GetValue(IconPathProperty);
            set => SetValue(IconPathProperty, value);
        }

        static MyPin()
        {
            IconPathProperty = BindableProperty.Create(nameof(IconPath), typeof(string), typeof(MyPin));
        }
    }
}
