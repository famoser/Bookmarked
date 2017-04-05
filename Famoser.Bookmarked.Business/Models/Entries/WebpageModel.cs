using System;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Entries.Base;

namespace Famoser.Bookmarked.Business.Models.Entries
{
    public class WebpageModel : ContentModel
    {
        private Uri _webpageUrl;
        public Uri WebpageUrl
        {
            get { return _webpageUrl; }
            set { Set(ref _webpageUrl, value); }
        }

        public override ContentType GetContentType()
        {
            return ContentType.Webpage;
        }
    }
}
