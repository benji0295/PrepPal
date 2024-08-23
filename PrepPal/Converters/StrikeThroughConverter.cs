using Microsoft.Maui.Controls;
using System;
using System.Globalization;
using System.Net.Mime;

namespace PrepPal.Converters;

public class StrikeThroughConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isChecked && isChecked)
        {
            return TextDecorations.Strikethrough;
        }

        return TextDecorations.None;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}