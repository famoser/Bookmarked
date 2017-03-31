using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;

namespace Famoser.Bookmarked.View.ViewModels.Folder.Base
{
    public class FolderViewModel : BaseViewModel
    {
        protected readonly IFolderRepository _folderRepository;
        protected readonly IHistoryNavigationService _navigationService;

        public FolderViewModel(IFolderRepository folderRepository, IHistoryNavigationService navigationService)
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
            get { return _folder; }
            set { Set(ref _folder, value); }
        }
    }
}
