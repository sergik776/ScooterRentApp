using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ScooterRent.Hardware.Server.WPF
{
    internal class UshortToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string ushortValue)
            {
                int totalSeconds = int.Parse(ushortValue);
                TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
                StringBuilder SB = new StringBuilder();
                if (timeSpan.Hours != 0) SB.Append($"{timeSpan.Hours}ч ");
                if (timeSpan.Minutes != 0) SB.Append($"{timeSpan.Minutes}м ");
                if (timeSpan.Seconds != 0) SB.Append($"{timeSpan.Seconds}с");
                return SB.ToString();
            }

            return TimeSpan.Zero; // Возвращаем TimeSpan.Zero, если значение не является ushort.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                return (ushort)timeSpan.TotalSeconds;
            }

            return (ushort)0; // Возвращаем 0, если значение не является TimeSpan.
        }
    }

    internal class UshortToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string ushortValue)
            {
                int totalSeconds = int.Parse(ushortValue);
                if(totalSeconds > 0)
                {
                    return new SolidColorBrush(System.Windows.Media.Color.FromRgb(255,0,0));
                }
            }
            return new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 0, 128));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
