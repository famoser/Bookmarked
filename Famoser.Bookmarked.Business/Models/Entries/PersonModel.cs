using Famoser.Bookmarked.Business.Enum;

namespace Famoser.Bookmarked.Business.Models.Entries
{
    public class PersonModel : NoteModel
    {
        private string _birthDay;
        public string BirthDay
        {
            get { return _birthDay; }
            set { Set(ref _birthDay, value); }
        }

        public override ContentType GetContentType()
        {
            return ContentType.Person;
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
            BirthDay = "01.01.1990";
        }
    }
}
