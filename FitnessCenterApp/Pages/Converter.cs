using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using FitnessCenterApp.Model;

namespace FitnessCenterApp.Pages
{
    [ValueConversion(typeof(int), typeof(BitmapSource))]
    public class IdToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(int)) return null;
            BitmapSource source;
            (source, _) = DBmanagement.LoadPhoto((int)value);
            return source;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class TrainerToWorkLengthConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value[0] == null || value[1] == null || 
                value[0].GetType() != typeof(DateTime) || value[1].GetType() != typeof(int)) return null;
            return ((int)((DateTime.Now - (DateTime)value[0]).Days / 365.2425) + (int)value[1]).ToString();
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return (object[])DependencyProperty.UnsetValue;
        }
    }
}
