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
            get { return _searchTerm; }
            set
            {
                if (Set(ref _searchTerm, value))
                {
                    SearchResults = _folderRepository.SearchEntry(SearchTerm);
                }
            }
        }

        private ObservableCollection<EntryModel> _searchResults;
        public ObservableCollection<EntryModel> SearchResults
        {
            get { return _searchResults; }
            set { Set(ref _searchResults, value); }
        }
    }
}
