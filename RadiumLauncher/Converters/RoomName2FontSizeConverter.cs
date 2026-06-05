using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RadiumLauncher.Converters
{
    public class RoomName2FontSizeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            double defaultSize = 14;
            if (parameter != null && double.TryParse(parameter.ToString(), out double parsedSize))
            {
                defaultSize = parsedSize;
            }

            if (value is string roomName && !string.IsNullOrEmpty(roomName))
            {
                return defaultSize;
            }

            return defaultSize + 4;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}