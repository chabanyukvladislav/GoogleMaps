using System;
using System.Globalization;
using Xamarin.Forms;

namespace Maps.Converters
{
    internal class ToNormalDurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var duration = int.Parse(value.ToString());
            var timeSpan = new TimeSpan(0, 0, duration);
            var days = "";
            if (timeSpan.Days != 0)
            {
                days = timeSpan.Days + "d ";
            }

            var hours = "";
            if (timeSpan.Hours != 0)
            {
                hours = timeSpan.Hours + " h ";
            }

            var minutes = "";
            if (timeSpan.Minutes != 0)
            {
                minutes = timeSpan.Minutes + " m ";
            }

            return days + hours + minutes + timeSpan.Seconds + " s";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
