using System.Collections.Generic;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Helper;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.Bookmarked.View.ViewModels.Entry;
using Famoser.Bookmarked.View.ViewModels.Folder;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        private readonly IFolderRepository _folderRepository;
        private readonly INavigationService _navigationService;

        public NavigationViewModel(IFolderRepository folderRepository, INavigationService navigationService)
        {
            _folderRepository = folderRepository;
            _navigationService = navigationService;
            SelectedFolder = _folderRepository.GetRootFolder();
        }

        private FolderModel _selectedFolder;
        public FolderModel SelectedFolder
        {
            get { return _selectedFolder; }
            set { Set(ref _selectedFolder, value); }
        }

        public ICommand RefreshCommand => new LoadingRelayCommand(() => _folderRepository.SyncAsnyc());
        public ICommand HelpCommand => new LoadingRelayCommand(() => _navigationService.NavigateTo(PageKeys.Info.ToString()));

        public ICommand SelectFolderCommand => new LoadingRelayCommand<FolderModel>(c =>
        {
            var folder = SelectedFolder;
            _navigationService.FakeNavigation(() => SelectedFolder = folder);
            SelectedFolder = c;
        });

        public ICommand AddFolderCommand => new LoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(PageKeys.AddFolder.ToString());
            var folder = _folderRepository.CreateFolder(SelectedFolder);
            SimpleIoc.Default.GetInstance<AddFolderViewModel>().SetFolder(folder);
        });

        public ICommand AddContentTypeCommand => new LoadingRelayCommand<ContentTypeModel>((cm) =>
        {
            _navigationService.NavigateTo(cm.AddPageKey.ToString());
            var entry = _folderRepository.CreateEntry(SelectedFolder, cm.ContentType);
            cm.SetEntryToViewModel(entry, CrudState.Add);
        });

        public ICommand EditFolderCommand => new LoadingRelayCommand<FolderModel>(c =>
        {
            _navigationService.NavigateTo(PageKeys.EditFolder.ToString());
            SimpleIoc.Default.GetInstance<EditFolderViewModel>().SetFolder(c);
        }, (c) => c?.ParentIds?.Count != 0);

        public ICommand SelectEntryCommand => new LoadingRelayCommand<EntryModel>(c =>
        {
            if (c.ContentType == ContentType.Webpage)
            {
                _navigationService.NavigateTo(PageKeys.ViewWebpage.ToString());
                SimpleIoc.Default.GetInstance<WebpageViewModel>().SetEntry(c, CrudState.View);
            }
        });

        public List<ContentTypeModel> ContentTypeModels { get; set; } = ContentHelper.GetContentTypeModels();
    }
}
