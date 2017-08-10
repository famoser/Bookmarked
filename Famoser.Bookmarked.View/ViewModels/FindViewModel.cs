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
    }
}
