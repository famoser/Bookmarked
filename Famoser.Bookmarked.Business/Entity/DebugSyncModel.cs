using System;
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
