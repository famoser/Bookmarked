using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.SyncApi.Enums;
using Famoser.SyncApi.Models.Interfaces;

namespace Famoser.Bookmarked.Business.Entity
{
    public class DebugSyncModel : ISyncActionInformation
    {
        public void SetSyncActionResult(SyncActionError result)
        {
            result.ToString();
        }

        public void SetSyncActionException(Exception exception)
        {
            exception.ToString();
        }
    }
}
