using System;
using System.Collections.ObjectModel;
using Famoser.Bookmarked.Business.Models.Base;
using Famoser.SyncApi.Models.Interfaces;

namespace Famoser.Bookmarked.Business.Models
{
    public class Entry : BaseModel, ISyncModel
    {
        private Uri _webpageUrl;
        public Uri WebpageUrl
        {
            get { return _webpageUrl; }
            set { Set(ref _webpageUrl, value); }
        }


        #region SyncApi implementation
        public string GetClassIdentifier()
        {
            return typeof(Entry).Name;
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
