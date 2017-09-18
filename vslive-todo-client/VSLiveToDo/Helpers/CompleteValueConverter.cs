using System;
using System.Globalization;
using Xamarin.Forms;

namespace VSLiveToDo.Helpers
{
    public class CompleteValueConverter : IValueConverter
    {
        public CompleteValueConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool completedVal))
                return Color.Black;

            return completedVal ? Color.Green : Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
