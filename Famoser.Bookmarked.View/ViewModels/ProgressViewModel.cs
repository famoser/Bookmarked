using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class ProgressViewModel : BaseViewModel, ISimpleProgressService
    {
        private readonly IDispatchService _dispatchService;
        public ProgressViewModel(IDispatchService dispatchService)
        {
            _dispatchService = dispatchService;
            if (IsInDesignMode)
            {
                ShowProgress = true;
                ActiveProgress = 8;
                MaxProgress = 20;
            }
        }

        public void InitializeProgressBar(int total)
        {
            _dispatchService.CheckBeginInvokeOnUi(() =>
            {
                ShowProgress = true;
                ActiveProgress = 0;
                MaxProgress = total;
            });
        }

        public void IncrementProgress()
        {
            _dispatchService.CheckBeginInvokeOnUi(() =>
            {
                ActiveProgress++;
            });
        }

        public void HideProgress()
        {
            _dispatchService.CheckBeginInvokeOnUi(() =>
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
