using Famoser.Bookmarked.Business.Enum;
using GalaSoft.MvvmLight;

namespace Famoser.Bookmarked.Business.Models.Entries.Base
{
    public abstract class ContentModel : ObservableObject
    {
        public abstract ContentType GetContentType();
    }
}
