using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.ViewModels.Folder.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;

namespace Famoser.Bookmarked.View.ViewModels.Folder
{
    public class ViewFolderViewModel : FolderViewModel
    {
        public ViewFolderViewModel(IFolderRepository folderRepository, IHistoryNavigationService navigationService) : base(folderRepository, navigationService)
        {
        }

        public ICommand EditEntryCommand => new LoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(Pages.EditFolder.ToString());
        });

        public ICommand DeleteEntryCommand => new LoadingRelayCommand(async () =>
        {
            await _folderRepository.RemoveFolderAsync(Folder);
            _navigationService.GoBack();
        });
    }
}
