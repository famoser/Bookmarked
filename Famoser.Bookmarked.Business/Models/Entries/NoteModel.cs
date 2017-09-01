using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Entries.Base;

namespace Famoser.Bookmarked.Business.Models.Entries
{
    public class NoteModel : ContentModel
    {
        private string _privateNote;
        public string PrivateNote
        {
            get { return _privateNote; }
            set { Set(ref _privateNote, value); }
        }

        public override ContentType GetContentType()
        {
            return ContentType.Note;
        }

        public override void SetDefaultValues()
        {
            //no defaults
        }

        public override void SetExampleValues()
        {
            PrivateNote = "my cat is green!";
        }
    }
}
