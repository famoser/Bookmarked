using Famoser.Bookmarked.Business.Enum;
using GalaSoft.MvvmLight;

namespace Famoser.Bookmarked.Business.Models.Entries.Base
{
    public abstract class ContentModel : ObservableObject
    {
        public abstract ContentType GetContentType();
        public abstract void SetDefaultValues();
        public abstract void SetExampleValues();
        public abstract CsvExportEntry ConvertToCsvExportEntry();
    }
}
