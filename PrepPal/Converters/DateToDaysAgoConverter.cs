using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace PrepPal.Converters
{
    public class DateToDaysAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime lastBoughtDate)
            {
                var daysAgo = (DateTime.Now - lastBoughtDate).TotalDays;
                return $"{Math.Floor(daysAgo)} days ago";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}