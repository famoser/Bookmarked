using System.Collections.ObjectModel;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class FindViewModel : BaseViewModel
    {
        private IFolderRepository _folderRepository;

        public FindViewModel(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (Set(ref _searchTerm, value))
                {
                    Entries = _folderRepository.SearchEntry(SearchTerm);
                    Folders = _folderRepository.SearchFolder(SearchTerm);
                }
            }
        }

        private ObservableCollection<EntryModel> _entries;
        public ObservableCollection<EntryModel> Entries
        {
            get => _entries;
            set => Set(ref _entries, value);
        }

        private ObservableCollection<FolderModel> _folders;
        public ObservableCollection<FolderModel> Folders
        {
            get => _folders;
            set => Set(ref _folders, value);
        }
    }
}
