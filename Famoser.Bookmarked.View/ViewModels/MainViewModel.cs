using System.Collections.ObjectModel;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;
using GalaSoft.MvvmLight.Messaging;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IHistoryNavigationService _navigationService;

        public MainViewModel(IFolderRepository folderRepository, IHistoryNavigationService navigationService)
        {
            _folderRepository = folderRepository;
            _navigationService = navigationService;
        }

        public ObservableCollection<Folder> Folders => _folderRepository.GetRootFolder().Folders;
        

        private Folder _selectedFolder;
        public Folder SelectedFolder
        {
            get { return _selectedFolder; }
            set { Set(ref _selectedFolder, value); }
        }

        public ICommand RefreshCommand => new LoadingRelayCommand(() => _folderRepository.SyncAsnyc());

        public ICommand SelectFolderCommand => new LoadingRelayCommand<Folder>((c) =>
        {
            _navigationService.NavigateTo(Pages.ViewFolder.ToString());
            Messenger.Default.Send(c, Messages.Select);
        });

        public ICommand AddFolderCommand => new LoadingRelayCommand<Folder>((c) =>
        {
            _navigationService.NavigateTo(Pages.AddEditFolder.ToString());
            Messenger.Default.Send(new Folder(), Messages.Select);
        });
    }
}
