using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.ViewModels.Folder.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;

namespace Famoser.Bookmarked.View.ViewModels.Folder
{
    public class AddFolderViewModel : FolderViewModel
    {
        public AddFolderViewModel(IFolderRepository folderRepository, IHistoryNavigationService navigationService) : base(folderRepository, navigationService)
        {
        }

        public ICommand SaveEntryCommand => new LoadingRelayCommand(async () =>
        {
            await _folderRepository.SaveFolderAsync(Folder);
            _navigationService.GoBack();
            _navigationService.NavigateTo(Pages.ViewFolder.ToString());
        });
    }
}
