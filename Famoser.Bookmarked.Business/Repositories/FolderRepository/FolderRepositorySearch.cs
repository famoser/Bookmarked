using System.Collections.ObjectModel;
using System.Linq;
using Famoser.Bookmarked.Business.Models;

namespace Famoser.Bookmarked.Business.Repositories.FolderRepository
{
    public partial class FolderRepository
    {
        public ObservableCollection<EntryModel> SearchEntry(string searchTerm)
        {
            if (searchTerm.Length < 3)
            {
                return new ObservableCollection<EntryModel>();
            }
            return new ObservableCollection<EntryModel>(_entries.Where(e => e.Name.Contains(searchTerm)));
        }
    }
}
