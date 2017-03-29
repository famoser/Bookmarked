using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Base;
using Famoser.SyncApi.Models.Interfaces;
using Newtonsoft.Json;

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
        
        #region SyncApi implementation
        public override string GetClassIdentifier()
        {
            return typeof(EntryModel).Name;
        }
        #endregion
    }
}
