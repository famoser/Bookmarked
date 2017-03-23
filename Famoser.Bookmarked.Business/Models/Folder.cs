using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Famoser.Bookmarked.Business.Models.Base;
using Famoser.SyncApi.Models.Interfaces;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Models
{
    public class Folder : BaseModel, ISyncModel
    {
        [JsonIgnore]
        public ObservableCollection<Folder> Children { get; set; } = new ObservableCollection<Folder>();

        [JsonIgnore]
        public Folder Parent { get; set; }

        internal Guid ParentId { get; set; }

        private bool _isDeleted;
        internal bool IsDeleted
        {
            get { return _isDeleted; }
            set { Set(ref _isDeleted, value); }
        }

        [OnSerializing]
        internal override void OnSerializingMethod(StreamingContext context)
        {
            ParentId = Parent.GetId();
            base.OnSerializingMethod(context);
        }

        #region SyncApi implementation
        public virtual string GetClassIdentifier()
        {
            return typeof(Folder).Name;
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
