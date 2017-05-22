using System;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Services.Interfaces;

namespace Famoser.Bookmarked.View.Services
{
    class ViewService : IViewService
    {
        private readonly IInteractionService _interactionService;

        public ViewService(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        public void CheckBeginInvokeOnUi(Action action)
        {
            _interactionService.CheckBeginInvokeOnUi(action);
        }
    }
}
