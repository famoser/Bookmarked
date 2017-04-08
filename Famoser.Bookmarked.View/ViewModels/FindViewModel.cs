using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    SearchItems();
                }
            }
        }

        private async void SearchItems()
        {

        }
    }
}
