using System;
using System.Collections.ObjectModel;
using Famoser.LectureSync.View.Services.Interfaces;
using Famoser.LectureSync.View.ViewModels.Base;
using Famoser.SyncApi.Api.Communication.Request.Base;
using Famoser.SyncApi.Enums;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Models.Interfaces;
using Famoser.SyncApi.Services.Interfaces;

namespace Famoser.LectureSync.View.ViewModels
{
    public class ApiViewModel : BaseViewModel, IApiTraceService
    {
        private readonly IInteractionService _interactionService;
        public ApiViewModel(IInteractionService interactionService)
        {
            _interactionService = interactionService;
            if (IsInDesignModeStatic)
            {
                var syncActionInfo = new SyncActionInformation(SyncAction.CreateUser);
                syncActionInfo.SetSyncActionResult(SyncActionError.None);
                SyncActionInformations.Add(syncActionInfo);
                syncActionInfo = new SyncActionInformation(SyncAction.CreateDevice);
                syncActionInfo.SetSyncActionResult(SyncActionError.None);
                SyncActionInformations.Add(syncActionInfo);
                syncActionInfo = new SyncActionInformation(SyncAction.SaveEntity);
                SyncActionInformations.Add(syncActionInfo);
            }
        }

        private int _requestCount;
        public int RequestCount
        {
            get { return _requestCount; }
            set { Set(ref _requestCount, value); }
        }

        private ObservableCollection<SyncActionInformation> _syncActionInformations = new ObservableCollection<SyncActionInformation>();
        public ObservableCollection<SyncActionInformation> SyncActionInformations
        {
            get { return _syncActionInformations; }
            set { Set(ref _syncActionInformations, value); }
        }

        public ISyncActionInformation CreateSyncActionInformation(SyncAction action)
        {
            var sa = new SyncActionInformation(action);
            _interactionService.CheckBeginInvokeOnUi(() =>
            {
                SyncActionInformations.Add(sa);
            });
            return sa;
        }

        public void TraceSuccessfulRequest(BaseRequest request, string link)
        {
            _interactionService.CheckBeginInvokeOnUi(() =>
            {
                RequestCount++;
            });
        }

        public void LogException(Exception ex, object @from = null)
        {
            // i dont care
        }

        public void TraceFailedRequest(BaseRequest request, string link, string message)
        {
            RequestCount++;
        }
    }
}
