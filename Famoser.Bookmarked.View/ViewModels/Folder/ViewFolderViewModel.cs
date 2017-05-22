using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Folder.Base;

namespace Famoser.Bookmarked.View.ViewModels.Folder
{
    public class ViewFolderViewModel : FolderViewModel
    {
        public ViewFolderViewModel(IFolderRepository folderRepository, INavigationService navigationService) : base(folderRepository, navigationService)
        {
        }

        public ICommand EditEntryCommand => new MyLoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(PageKeys.EditFolder.ToString());
        });

        public ICommand DeleteEntryCommand => new MyLoadingRelayCommand(async () =>
        {
            await _folderRepository.MoveFolderToGarbageAsync(Folder);
            _navigationService.GoBack();
        });
    }
}
