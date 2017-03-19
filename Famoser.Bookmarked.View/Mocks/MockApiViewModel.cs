using System;
using System.Collections.ObjectModel;
using Famoser.LectureSync.View.ViewModels.Base;
using Famoser.SyncApi.Api.Communication.Request.Base;
using Famoser.SyncApi.Enums;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Models.Interfaces;
using Famoser.SyncApi.Services.Interfaces;

namespace Famoser.LectureSync.View.Mocks
{
    public class MockApiViewModel : BaseViewModel, IApiTraceService
    {
        public MockApiViewModel()
        {
            var obj = new SyncActionInformation(SyncAction.CreateUser);
            obj.SetSyncActionResult(SyncActionError.None);
            SyncActionInformations.Add(obj);
            obj = new SyncActionInformation(SyncAction.CreateDevice);
            obj.SetSyncActionResult(SyncActionError.None);
            SyncActionInformations.Add(obj);
            obj = new SyncActionInformation(SyncAction.GetDefaultCollection);
            obj.SetSyncActionResult(SyncActionError.None);
            SyncActionInformations.Add(obj);
            obj = new SyncActionInformation(SyncAction.SaveCollection);
            SyncActionInformations.Add(obj);

            RequestCount = 4;
        }
        private int _requestCount;
        public int RequestCount
        {
            get { return _requestCount; }
            set { Set(ref _requestCount, value); }
        }

        private ObservableCollection<SyncActionInformation> _syncActionInformations;
        public ObservableCollection<SyncActionInformation> SyncActionInformations
        {
            get { return _syncActionInformations; }
            set { Set(ref _syncActionInformations, value); }
        }

        public ISyncActionInformation CreateSyncActionInformation(SyncAction action)
        {
            var sa = new SyncActionInformation(action);
            SyncActionInformations.Add(sa);
            return sa;
        }

        public void TraceSuccessfulRequest(BaseRequest request, string link)
        {
            RequestCount++;
        }

        public void LogException(Exception ex, object @from = null)
        {
            // i dont care
        }

        public void TraceFailedRequest(BaseRequest request, string link, string message)
        {
            // i dont care
        }
    }
}
