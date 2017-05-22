using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

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
