using System;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Base;

namespace Famoser.Bookmarked.Business.Models
{
    public class EntryModel : ParentModel
    {
        private string _content;
        internal string Content
        {
            get { return _content; }
            set { Set(ref _content, value); }
        }

        private ContentType _contentType;
        public ContentType ContentType
        {
            get { return _contentType; }
            set { Set(ref _contentType, value); }
        }

        private Uri _iconUri;
        public Uri IconUri
        {
            get { return _iconUri; }
            set { Set(ref _iconUri, value); }
        }
        
        #region SyncApi implementation
        public override string GetClassIdentifier()
        {
            return typeof(EntryModel).Name;
        }
        #endregion
    }
}
