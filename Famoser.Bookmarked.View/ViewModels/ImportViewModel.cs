using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class ImportViewModel : BaseViewModel
    {
        private readonly IFolderRepository _folderRepository;
        private IApiService _apiService;
        private readonly ILoginService _loginService;
        private IInteractionService _interactionService;

        public ImportViewModel(IFolderRepository folderRepository, IApiService apiService, IInteractionService interactionService, ILoginService loginService)
        {
            _folderRepository = folderRepository;
            _apiService = apiService;
            _interactionService = interactionService;
            _loginService = loginService;
        }

        public ICommand ImportCommand => new MyLoadingRelayCommand(ImportAsync);

        private async Task ImportAsync()
        {
            var str = await _interactionService.GetFileContentAsync("bmd_data");
            if (str != null)
                await _folderRepository.ImportDataAsync(str);
        }

        public ICommand ExportCommand => new MyLoadingRelayCommand(ExportAsync);

        private async Task ExportAsync()
        {
            var exportString = await _folderRepository.ExportDataAsync();
            await _interactionService.SaveFileAsync(exportString, "bmd_data", "bookmarked_data");
        }

        /*
         * export / import credentials
         */
        public ICommand ExportCredentialsCommand => new MyLoadingRelayCommand(ExportCredentialsAsync);

        private async Task ExportCredentialsAsync()
        {
            var exportString = await _folderRepository.ExportCredentialsAsync();
            await _interactionService.SaveFileAsync(exportString, "bmd_cred", "bookmarked_credentials");
        }

        public ICommand ImportCredentialsCommand => new MyLoadingRelayCommand(ImportCredentialsAsync);

        private async Task ImportCredentialsAsync()
        {
            var str = await _interactionService.GetFileContentAsync("bmd_cred");
            if (await _folderRepository.ImportCredentialsAsync(str))
            {
                await _interactionService.ShowMessageAsync("import successful, the application will close now");
                _interactionService.CloseApplication();
            }
            else
            {
                await _interactionService.ShowMessageAsync("the import failed, are you sure the password is correct?");
            }
        }

        /*
         * danger functions
         */
        public ICommand ClearCacheCommand => new MyLoadingRelayCommand(ClearCacheAsync);

        private async Task ClearCacheAsync()
        {
            if (await _interactionService.ClearCacheAsync())
            {
                await _interactionService.ShowMessageAsync("cache emptied successful, the application will close now");
                _interactionService.CloseApplication();
            }
            else
            {
                await _interactionService.ShowMessageAsync("cache could not be emptied :(");
            }
        }

        public ICommand ResetApplicationCommand => new MyLoadingRelayCommand(ResetApplicationAsync);

        private async Task ResetApplicationAsync()
        {
            if (await _folderRepository.ClearAllDataAsync())
            {
                await _interactionService.ClearCacheAsync();
                await _interactionService.ShowMessageAsync("reset successful, the application will close now");
                _interactionService.CloseApplication();
            }
            else
            {

            }
        }
    }
}
