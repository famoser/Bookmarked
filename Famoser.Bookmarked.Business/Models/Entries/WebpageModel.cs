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

        public override void SetDefaultValues()
        {
            //no defaults
        }

        public override void SetExampleValues()
        {
            WebpageUrl = new Uri("http://famoser.ch");
        }

        public override CsvExportEntry ConvertToCsvExportEntry()
        {
            var entry = base.ConvertToCsvExportEntry();

            entry.WebpageUrl = WebpageUrl?.ToString();

            return entry;
        }
    }
}
