using System;
using Famoser.Bookmarked.Business.Enum;

namespace Famoser.Bookmarked.Business.Models.Entries
{
    public class WebpageModel : NoteModel
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
