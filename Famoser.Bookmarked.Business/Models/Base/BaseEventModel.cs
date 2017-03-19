namespace Famoser.LectureSync.Business.Models.Base
{
    public class BaseEventModel : BaseModel
    {
        private string _lecturer;
        public string Lecturer
        {
            get { return _lecturer; }
            set { Set(ref _lecturer, value); }
        }

        private string _place;
        public string Place
        {
            get { return _place; }
            set { Set(ref _place, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }
    }
}
