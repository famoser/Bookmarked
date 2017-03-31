﻿using System;
using System.Collections.Generic;
using Famoser.SyncApi.Models.Interfaces;

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
