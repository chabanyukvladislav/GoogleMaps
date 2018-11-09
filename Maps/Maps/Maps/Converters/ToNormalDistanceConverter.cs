using System;
using System.Globalization;
using Xamarin.Forms;

namespace Maps.Converters
{
    internal class ToNormalDistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var distance = double.Parse(value.ToString());
            return (distance / 1000) + " km";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
