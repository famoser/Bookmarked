using Windows.ApplicationModel.Resources;

namespace Famoser.Bookmarked.Presentation.Universal.Strings
{
    public class LocalizedStrings
    {
        public string this[string key] => ResourceLoader.GetForViewIndependentUse().GetString(key);
    }
}
