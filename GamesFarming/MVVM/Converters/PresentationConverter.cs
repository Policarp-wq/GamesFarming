using System;
using System.Globalization;
using System.Windows.Data;

namespace GamesFarming.MVVM.Converters
{
    internal class PresentationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Tuple<object, object> tuple = Tuple.Create(values[0], values[1]);
                return tuple;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
