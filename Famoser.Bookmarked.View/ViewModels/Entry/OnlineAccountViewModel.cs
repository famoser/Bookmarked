using System.Windows.Input;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Entry.Base;
using GalaSoft.MvvmLight.Command;

namespace Famoser.Bookmarked.View.ViewModels.Entry
{
    public class OnlineAccountViewModel : WithUrlViewModel<OnlineAccountModel>
    {
        private IInteractionService _interactionService;
        public OnlineAccountViewModel(IFolderRepository folderRepository, INavigationService navigationService, IApiService apiService, IInteractionService interactionService) : base(folderRepository, navigationService, apiService)
        {
            _interactionService = interactionService;

            CopyPasswordToClipboard = new RelayCommand(() => _interactionService.CopyToClipboard(SelectedEntryContent.Password));
        }
        public ICommand CopyPasswordToClipboard { get; }
    }
}
