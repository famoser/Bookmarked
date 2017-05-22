using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Folder.Base;

namespace Famoser.Bookmarked.View.ViewModels.Folder
{
    public class AddFolderViewModel : FolderViewModel
    {
        public AddFolderViewModel(IFolderRepository folderRepository, INavigationService navigationService) : base(folderRepository, navigationService)
        {
        }

        public ICommand SaveFolderCommand => new MyLoadingRelayCommand(async () =>
        {
            await _folderRepository.SaveFolderAsync(Folder);
            _navigationService.GoBack();
        });
    }
}
