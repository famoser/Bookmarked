using System;
using System.Collections.ObjectModel;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.SyncApi.Api.Communication.Request.Base;
using Famoser.SyncApi.Enums;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Models.Interfaces;
using Famoser.SyncApi.Services.Interfaces;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class ApiViewModel : BaseViewModel, IApiTraceService
    {
        private readonly IDispatchService _dispatchService;
        public ApiViewModel(IDispatchService dispatchService)
        {
            _dispatchService = dispatchService;
            if (IsInDesignModeStatic)
            {
                var syncActionInfo = new SyncActionInformation(SyncAction.CreateUser);
                syncActionInfo.SetSyncActionResult(SyncActionError.None);
                SyncActionInformation.Add(syncActionInfo);
                syncActionInfo = new SyncActionInformation(SyncAction.CreateDevice);
                syncActionInfo.SetSyncActionResult(SyncActionError.None);
                SyncActionInformation.Add(syncActionInfo);
                syncActionInfo = new SyncActionInformation(SyncAction.SaveEntity);
                SyncActionInformation.Add(syncActionInfo);
            }
        }

        private int _requestCount;
        public int RequestCount
        {
            get => _requestCount;
            set => Set(ref _requestCount, value);
        }

        private ObservableCollection<SyncActionInformation> _syncActionInformation = new ObservableCollection<SyncActionInformation>();
        public ObservableCollection<SyncActionInformation> SyncActionInformation
        {
            get => _syncActionInformation;
            set => Set(ref _syncActionInformation, value);
        }

        public ISyncActionInformation CreateSyncActionInformation(SyncAction action)
        {
            var sa = new SyncActionInformation(action);
            _dispatchService.CheckBeginInvokeOnUi(() =>
            {
                SyncActionInformation.Add(sa);
            });
            return sa;
        }

        public void TraceSuccessfulRequest(BaseRequest request, string link)
        {
            _dispatchService.CheckBeginInvokeOnUi(() =>
            {
                RequestCount++;
            });
        }

        public void LogException(Exception ex, object from = null)
        {
            // i dont care
        }

        public void TraceFailedRequest(BaseRequest request, string link, string message)
        {
            RequestCount++;
        }
    }
}
