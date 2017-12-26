using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Famoser.SyncApi.Api.Communication.Request.Base;
using Famoser.SyncApi.Enums;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Models.Interfaces;
using Famoser.SyncApi.Services.Interfaces;

namespace Famoser.Bookmarked.Presentation.Webpage.Platform
{
    /// <summary>
    /// we dont care about traces in our simple online viewer
    /// </summary>
    public class ApiTraceService : IApiTraceService
    {
        public void LogException(Exception ex, object @from = null)
        {

        }

        public ISyncActionInformation CreateSyncActionInformation(SyncAction action)
        {
            return new SyncActionInformation(action);
        }

        public void TraceSuccessfulRequest(BaseRequest request, string link)
        {

        }

        public void TraceFailedRequest(BaseRequest request, string link, string message)
        {

        }
    }
}