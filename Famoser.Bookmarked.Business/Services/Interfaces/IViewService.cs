using System;

namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IViewService
    {
        void CheckBeginInvokeOnUi(Action action);
    }
}
