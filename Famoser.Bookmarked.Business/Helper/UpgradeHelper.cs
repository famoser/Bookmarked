using Famoser.Bookmarked.Business.Models.Entries;

namespace Famoser.Bookmarked.Business.Helper
{
    public class UpgradeHelper
    {
        public static void WriteValues(NoteModel source, NoteModel target)
        {
            target.PrivateNote = source.PrivateNote;
        }
        public static void WriteValues(WebpageModel source, OnlineAccountModel target)
        {
            WriteValues(source as NoteModel, target);
            target.WebpageUrl = source.WebpageUrl;
        }
    }
}
