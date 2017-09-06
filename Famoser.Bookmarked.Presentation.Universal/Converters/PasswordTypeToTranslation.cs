using System;
using Windows.UI.Xaml.Data;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Presentation.Universal.Strings;

namespace Famoser.Bookmarked.Presentation.Universal.Converters
{
    public class PasswordTypeToTranslation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var pwType = (PasswordType) value;
            var append = parameter is string ? (string) parameter : "";
            var prepend = "GeneratePasswordTo";
            string middle;
            switch (pwType)
            {
                case PasswordType.ToCommunicate:
                    middle = "Communicate";
                    break;
                case PasswordType.ToCopy:
                    middle = "Copy";
                    break;
                case PasswordType.ToType:
                    middle = "Type";
                    break;
                case PasswordType.ToRemember:
                    middle = "Remember";
                    break;
                default:
                    throw new Exception("missing translation");
            }
            return new LocalizedStrings()[prepend + middle + append];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
