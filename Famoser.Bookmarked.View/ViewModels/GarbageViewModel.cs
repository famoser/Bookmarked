using System.Collections.Generic;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Helper;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class GarbageViewModel : BaseViewModel
    {
        private readonly IFolderRepository _folderRepository;
        private readonly INavigationService _navigationService;

        public GarbageViewModel(IFolderRepository folderRepository, INavigationService navigationService)
        {
            _folderRepository = folderRepository;
            _navigationService = navigationService;
            GarbageFolder = _folderRepository.GetGarbageFolder();
        }

        private FolderModel _garbageFolder;
        public FolderModel GarbageFolder
        {
            get { return _garbageFolder; }
            set { Set(ref _garbageFolder, value); }
        }

        public ICommand HelpCommand => new MyLoadingRelayCommand(() => _navigationService.NavigateTo(PageKeys.Info.ToString()));

        public ICommand RemoveFolderCommand => new MyLoadingRelayCommand<FolderModel>(c => _folderRepository.RemoveFolderAsync(c));

        public ICommand RecoverFolderCommand => new MyLoadingRelayCommand<FolderModel>(c => _folderRepository.MoveFolderOutOfGarbageAsync(c));

        public ICommand RemoveEntryCommand => new MyLoadingRelayCommand<EntryModel>(c => _folderRepository.RemoveEntryAsync(c));

        public ICommand RecoverEntryCommand => new MyLoadingRelayCommand<EntryModel>(c => _folderRepository.MoveEntryOutOfGarbageAsync(c));

        public List<ContentTypeModel> ContentTypeModels { get; set; } = ContentHelper.GetContentTypeModels();
    }
}
