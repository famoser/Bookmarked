using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Helper;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.Bookmarked.View.ViewModels.Folder;
using Famoser.Bookmarked.View.ViewModels.Interface;
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

            if (IsInDesignMode)
            {
                InSearchMode = true;
            }
        }

        private FolderModel _selectedFolder;
        public FolderModel SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                if (Set(ref _selectedFolder, value))
                {
                    RaisePropertyChanged(() => TotalFolders);
                    RaisePropertyChanged(() => TotalEntries);
                }
            }
        }

        public int TotalFolders => SelectedFolder.TotalFoldersInStructure;
        public int TotalEntries => SelectedFolder.TotalEntriesInStructure;

        public ICommand RefreshCommand => new MyLoadingRelayCommand(() => _folderRepository.SyncAsync());
        public ICommand HelpCommand => new MyLoadingRelayCommand(() => _navigationService.NavigateTo(PageKeys.Info.ToString()));

        public ICommand SelectFolderCommand => new MyLoadingRelayCommand<FolderModel>(c =>
        {
            var folder = SelectedFolder;
            _navigationService.FakeNavigation(() => SelectedFolder = folder);
            SelectedFolder = c;
        });

        public Task<bool> MoveToNewFolderAsync(EntryModel entry, FolderModel newFolder)
        {
            var oldFolder = SelectedFolder;
            return _folderRepository.ReplaceFolderOfEntryAsync(entry, oldFolder, newFolder);
        }

        public Task<bool> MoveOneFolderUpAsync(EntryModel entry)
        {
            var oldFolder = SelectedFolder;
            _navigationService.GoBack();
            var newFolder = SelectedFolder;
            return _folderRepository.ReplaceFolderOfEntryAsync(entry, oldFolder, newFolder);
        }

        public Task<bool> MoveToNewFolderAsync(FolderModel folder, FolderModel newFolder)
        {
            var oldFolder = SelectedFolder;
            return _folderRepository.ReplaceFolderOfEntryAsync(folder, oldFolder, newFolder);
        }

        public Task<bool> MoveOneFolderUpAsync(FolderModel folder)
        {
            var oldFolder = SelectedFolder;
            _navigationService.GoBack();
            var newFolder = SelectedFolder;
            return _folderRepository.ReplaceFolderOfEntryAsync(folder, oldFolder, newFolder);
        }

        public ICommand AddFolderCommand => new MyLoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(PageKeys.AddFolder.ToString());
            var folder = _folderRepository.CreateFolder(SelectedFolder);
            SimpleIoc.Default.GetInstance<AddFolderViewModel>().SetFolder(folder);
        });

        public ICommand FindCommand => new MyLoadingRelayCommand(() =>
        {
            InSearchMode = !InSearchMode;
        });

        public ICommand AddContentTypeCommand => new MyLoadingRelayCommand<ContentTypeModel>((cm) =>
        {
            if (cm != null)
            {
                _navigationService.NavigateTo(cm.AddPageKey.ToString());
                var entry = _folderRepository.CreateEntry(SelectedFolder, cm.ContentType);
                ((IEntryViewModel)SimpleIoc.Default.GetInstance(cm.ViewModelType)).SetEntry(entry, CrudState.Add);
            }
        });

        public ICommand EditFolderCommand => new MyLoadingRelayCommand<FolderModel>(c =>
        {
            _navigationService.NavigateTo(PageKeys.EditFolder.ToString());
            SimpleIoc.Default.GetInstance<EditFolderViewModel>().SetFolder(c);
        }, (c) => c?.ParentIds?.Count != 0);

        public ICommand SelectEntryCommand => new MyLoadingRelayCommand<EntryModel>(c =>
        {
            var model = ContentHelper.GetContentTypeModel(c.ContentType);
            if (model != null)
            {
                _navigationService.NavigateTo(model.ViewPageKey.ToString());
                ((IEntryViewModel)SimpleIoc.Default.GetInstance(model.ViewModelType)).SetEntry(c, CrudState.Add);
            }
        });

        public List<ContentTypeModel> ContentTypeModels { get; } = ContentHelper.GetContentTypeModels();

        private bool _inSearchMode;
        public bool InSearchMode
        {
            get => _inSearchMode;
            set
            {
                if (Set(ref _inSearchMode, value))
                {
                    if (InTrashMode)
                    {
                        Set(ref _inTrashMode);
                        NavigationViewMode = NavigationViewMode.Search;
                    }
                    else
                    {
                        NavigationViewMode = NavigationViewMode.Default;
                    }
                }
            }
        }
        private bool _inTrashMode;
        public bool InTrashMode
        {
            get => _inTrashMode;
            set
            {
                if (Set(ref _inTrashMode, value))
                {
                    if (InTrashMode)
                    {
                        Set(ref _inSearchMode);
                        NavigationViewMode = NavigationViewMode.Trash;
                    }
                    else
                    {
                        NavigationViewMode = NavigationViewMode.Default;
                    }
                }
            }
        }

        private NavigationViewMode _navigationViewMode;
        public NavigationViewMode NavigationViewMode
        {
            get => _navigationViewMode;
            set => Set(ref _navigationViewMode, value);
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (Set(ref _searchTerm, value))
                {
                    SearchEntries = _folderRepository.SearchEntry(SearchTerm);
                    SearchFolders = _folderRepository.SearchFolder(SearchTerm);
                }
            }
        }

        private ObservableCollection<EntryModel> _searchEntries;
        public ObservableCollection<EntryModel> SearchEntries
        {
            get => _searchEntries;
            set => Set(ref _searchEntries, value);
        }

        private ObservableCollection<FolderModel> _searchFolders;
        public ObservableCollection<FolderModel> SearchFolders
        {
            get => _searchFolders;
            set => Set(ref _searchFolders, value);
        }
    }
}
