using System;
using Famoser.Bookmarked.Business.Services.Interfaces;

namespace Famoser.Bookmarked.Presentation.Webpage.Platform
{
    public class DispatchService : IDispatchService
    {
        public void CheckBeginInvokeOnUi(Action action)
        {
            action.Invoke();
        }
    }
}
