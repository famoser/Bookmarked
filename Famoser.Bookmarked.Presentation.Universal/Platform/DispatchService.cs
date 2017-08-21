using System;
using Famoser.Bookmarked.Business.Services.Interfaces;
using GalaSoft.MvvmLight.Threading;

namespace Famoser.Bookmarked.Presentation.Universal.Platform
{
    public class DispatchService : IDispatchService
    {
        public void CheckBeginInvokeOnUi(Action action)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(action);
        }
    }
}
