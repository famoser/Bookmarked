using Famoser.Bookmarked.Business.Enum;

namespace Famoser.Bookmarked.Business.Models.Entries
{
    public class BookModel : NoteModel
    {
        private string _isbn;
        public string ISBN
        {
            get { return _isbn; }
            set { Set(ref _isbn, value); }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set { Set(ref _author, value); }
        }

        public override ContentType GetContentType()
        {
            return ContentType.Book;
        }

        public void WriteProperties(NoteModel note)
        {

        }

        public override void SetDefaultValues()
        {
            //no defaults
        }

        public override void SetExampleValues()
        {
            Author = "Dennis Herman";
            ISBN = "6126371-123";
        }
    }
}
