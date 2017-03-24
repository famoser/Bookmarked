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
        internal List<Guid> ParentIds { get; set; }

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
