using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace PrepPal.Converters;

public class DateToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime lastBoughtDate)
        {
            var daysAgo = (DateTime.Now - lastBoughtDate).TotalDays;

            if (daysAgo <= 2)
            {
                return (Colors.Green); // Recently bought
            }
            else if (daysAgo <= 7)
            {
                return (Colors.Yellow); // A few days ago
            }
            else
            {
                return (Colors.Red); // Over a week ago
            }
        }

        return Colors.Gray; // Default color
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}