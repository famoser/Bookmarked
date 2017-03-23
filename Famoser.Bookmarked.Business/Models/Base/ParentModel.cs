using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Famoser.SyncApi.Models.Interfaces;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Models.Base
{
    public abstract class ParentModel : BaseModel, ISyncModel
    {
        [JsonIgnore]
        public Folder Parent { get; set; }

        internal Guid ParentId { get; set; }
        public int Order { get; set; }

        [OnSerializing]
        internal virtual void OnSerializingMethod(StreamingContext context)
        {
            ParentId = Parent.GetId();
        }

        #region SyncApi implementation
        private Guid _guid;
        public void SetId(Guid id)
        {
            _guid = id;
        }

        public Guid GetId()
        {
            return _guid;
        }

        public abstract string GetClassIdentifier();
        #endregion
    }
}
