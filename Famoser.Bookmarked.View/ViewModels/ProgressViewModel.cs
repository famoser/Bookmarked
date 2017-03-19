using Famoser.LectureSync.Business.Services.Interfaces;
using Famoser.LectureSync.View.Services.Interfaces;
using Famoser.LectureSync.View.ViewModels.Base;

namespace Famoser.LectureSync.View.ViewModels
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
            get { return _showProgress; }
            set { Set(ref _showProgress, value); }
        }

        private int _maxProgress;
        public int MaxProgress
        {
            get { return _maxProgress; }
            set { Set(ref _maxProgress, value); }
        }

        private int _activeProgress;
        public int ActiveProgress
        {
            get { return _activeProgress; }
            set { Set(ref _activeProgress, value); }
        }
    }
}
