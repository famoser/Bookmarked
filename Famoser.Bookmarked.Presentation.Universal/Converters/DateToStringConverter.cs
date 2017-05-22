using System;
using Windows.UI.Xaml.Data;

namespace Famoser.Bookmarked.Presentation.Universal.Converters
{
    class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTime) value;
            return date == DateTime.MinValue ? "-" : date.ToString("MM/yy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
