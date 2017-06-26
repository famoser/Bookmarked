using System;
using Windows.UI.Xaml.Data;

namespace Famoser.Bookmarked.Presentation.Universal.Converters
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
                var str = value as string;
                if (str != null)
                {
                    if (!str.StartsWith("http://") && !str.StartsWith("https://"))
                        return new Uri("http://" + str);
                    return new Uri(str);
                }
            }
            catch
            {
                // Uri may fails if invalid URI
            }
            return null;
        }
    }
}
