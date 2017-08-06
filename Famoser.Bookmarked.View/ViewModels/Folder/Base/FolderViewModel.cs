using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

namespace Famoser.Bookmarked.View.ViewModels.Folder.Base
{
    public class FolderViewModel : BaseViewModel
    {
        protected readonly IFolderRepository _folderRepository;
        protected readonly INavigationService _navigationService;

        public FolderViewModel(IFolderRepository folderRepository, INavigationService navigationService)
        {
            _folderRepository = folderRepository;
            _navigationService = navigationService;
        }

        internal void SetFolder(FolderModel folderModel)
        {
            Folder = folderModel;
        }

        private FolderModel _folder;
        public FolderModel Folder
        {
            get => _folder;
            set => Set(ref _folder, value);
        }
    }
}
