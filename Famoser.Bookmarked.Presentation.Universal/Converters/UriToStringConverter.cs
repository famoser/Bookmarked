using System;
using Windows.UI.Xaml.Data;

namespace Famoser.LectureSync.Presentation.Universal.Converters
{
    public class UriToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
                return value.ToString();
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return new Uri(value as string);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
