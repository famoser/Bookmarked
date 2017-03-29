using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.Bookmarked.View.ViewModels.Entry;
using Famoser.Bookmarked.View.ViewModels.Entry.Abstract;
using Famoser.Bookmarked.View.ViewModels.Folder;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IHistoryNavigationService _navigationService;
        private readonly Stack<FolderModel> _folderHistory = new Stack<FolderModel>();

        public NavigationViewModel(IFolderRepository folderRepository, IHistoryNavigationService navigationService)
        {
            _folderRepository = folderRepository;
            _navigationService = navigationService;
            _folderHistory.Push(_folderRepository.GetRootFolder());
            SelectedFolder = _folderHistory.Peek();
        }

        private FolderModel _selectedFolder;
        public FolderModel SelectedFolder
        {
            get { return _selectedFolder; }
            set { Set(ref _selectedFolder, value); }
        }

        public ICommand RefreshCommand => new LoadingRelayCommand(() => _folderRepository.SyncAsnyc());

        public ICommand SelectFolderCommand => new LoadingRelayCommand<FolderModel>(c =>
        {
            _folderHistory.Push(c);
            SelectedFolder = c;
        });

        public ICommand GoBackCommand => new LoadingRelayCommand<FolderModel>(c =>
        {
            SelectedFolder = _folderHistory.Pop();
        }, c => _folderHistory.Count > 1);

        public ICommand AddFolderCommand => new LoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(Pages.AddFolder.ToString());
            var folder = _folderRepository.CreateFolderAsync(SelectedFolder);
            SimpleIoc.Default.GetInstance<EditFolderViewModel>().SetFolder(folder);
        });

        public ICommand EditFolderCommand => new LoadingRelayCommand<FolderModel>(c =>
        {
            _navigationService.NavigateTo(Pages.EditFolder.ToString());
            SimpleIoc.Default.GetInstance<EditFolderViewModel>().SetFolder(c);
        });

        public ICommand ViewEntryCommand => new LoadingRelayCommand<EntryModel>(async c =>
        {
            if (c.ContentType == ContentType.Webpage)
            {
                _navigationService.NavigateTo(Pages.ViewWebpage.ToString());
                await SimpleIoc.Default.GetInstance<WebpageViewModel>().SetEntry(c, CrudState.View);
            }
        });
    }
}
