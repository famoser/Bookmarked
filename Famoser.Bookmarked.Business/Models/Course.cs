using System;
using System.Collections.ObjectModel;
using Famoser.LectureSync.Business.Models.Base;
using Famoser.SyncApi.Models.Interfaces;

namespace Famoser.LectureSync.Business.Models
{
    public class Course : BaseEventModel, ISyncModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private Uri _infoUrl;
        public Uri InfoUrl
        {
            get { return _infoUrl; }
            set { Set(ref _infoUrl, value); }
        }

        private Uri _webpageUrl;
        public Uri WebpageUrl
        {
            get { return _webpageUrl; }
            set { Set(ref _webpageUrl, value); }
        }

        private Uri _exerciseUrl;
        public Uri ExerciseUrl
        {
            get { return _exerciseUrl; }
            set { Set(ref _exerciseUrl, value); }
        }

        public ObservableCollection<Lecture> Lectures { get; set; } = new ObservableCollection<Lecture>();

        #region SyncApi implementation
        public string GetClassIdentifier()
        {
            return typeof(Course).Name;
        }

        private Guid _guid;
        public void SetId(Guid id)
        {
            _guid = id;
        }

        public Guid GetId()
        {
            return _guid;
        }
        #endregion
    }
}
