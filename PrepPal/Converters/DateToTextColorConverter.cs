using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace PrepPal.Converters;

public class DateToTextColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime lastBoughtDate)
        {
            var daysAgo = (DateTime.Now - lastBoughtDate).TotalDays;

            if (daysAgo <= 7 && daysAgo > 2)
            {
                return Colors.Black; // Text is black for yellow background
            }
            else
            {
                return Colors.Black; // Text is white for green or red background
            }
        }

        return Colors.Black; // Default color
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}