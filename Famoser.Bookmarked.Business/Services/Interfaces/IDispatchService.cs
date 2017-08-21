using System;

namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IDispatchService
    {
        void CheckBeginInvokeOnUi(Action action);
    }
}
