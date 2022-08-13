using System;
using System.Globalization;
using System.Windows.Data;

namespace Vexis.Client.Converters;

public class EnumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Enum.GetName(value.GetType(), value) ?? throw new InvalidOperationException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}