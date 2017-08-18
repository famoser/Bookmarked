using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Famoser.Bookmarked.Presentation.Universal.Strings
{
    public class LocalizedStrings
    {
        public string this[string key] => ResourceLoader.GetForViewIndependentUse().GetString(key);
    }
}
