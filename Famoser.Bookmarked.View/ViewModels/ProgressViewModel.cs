using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class ProgressViewModel : BaseViewModel, ISimpleProgressService
    {
        private readonly IInteractionService _interactionService;
        public ProgressViewModel(IInteractionService interactionService)
        {
            _interactionService = interactionService;
            if (IsInDesignMode)
            {
                ShowProgress = true;
                ActiveProgress = 8;
                MaxProgress = 20;
            }
        }

        public void InitializeProgressBar(int total)
        {
            _interactionService.CheckBeginInvokeOnUi(() =>
            {
                ShowProgress = true;
                ActiveProgress = 0;
                MaxProgress = total;
            });
        }

        public void IncrementProgress()
        {
            _interactionService.CheckBeginInvokeOnUi(() =>
            {
                ActiveProgress++;
            });
        }

        public void HideProgress()
        {
            _interactionService.CheckBeginInvokeOnUi(() =>
            {
                ShowProgress = false;
            });
        }

        private bool _showProgress;
        public bool ShowProgress
        {
            get => _showProgress;
            set => Set(ref _showProgress, value);
        }

        private int _maxProgress;
        public int MaxProgress
        {
            get => _maxProgress;
            set => Set(ref _maxProgress, value);
        }

        private int _activeProgress;
        public int ActiveProgress
        {
            get => _activeProgress;
            set => Set(ref _activeProgress, value);
        }
    }
}
