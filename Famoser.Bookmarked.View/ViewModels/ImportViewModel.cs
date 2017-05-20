using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Helper;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Logging;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class ImportViewModel : BaseViewModel
    {
        private IFolderRepository _folderRepository;
        private IApiService _apiService;

        public ImportViewModel(IFolderRepository folderRepository, IApiService apiService)
        {
            _folderRepository = folderRepository;
            _apiService = apiService;
        }

        public Task<bool> Import(string content)
        {
            return _folderRepository.ImportDataAsync(content);
        }

        public Task<string> Export()
        {
            return _folderRepository.ExportDataAsync();
        }
    }
}
