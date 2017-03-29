using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;

namespace Famoser.Bookmarked.View.ViewModels.Folder
{
    public class AddFolderViewModel : BaseViewModel
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IHistoryNavigationService _navigationService;

        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }
    }
}
