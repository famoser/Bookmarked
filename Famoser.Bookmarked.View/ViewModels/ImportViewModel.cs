using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Services.Interfaces;
using System;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.SyncApi.Storage.Roaming;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class ImportViewModel : BaseViewModel
    {
        private readonly IFolderRepository _folderRepository;
        private IApiService _apiService;
        private readonly IImportExportService _importExportService;
        private readonly ILoginService _loginService;
        private IInteractionService _interactionService;

        public ImportViewModel(IFolderRepository folderRepository, IApiService apiService, IImportExportService importExportService, IInteractionService interactionService, ILoginService loginService)
        {
            _folderRepository = folderRepository;
            _apiService = apiService;
            _importExportService = importExportService;
            _interactionService = interactionService;
            _loginService = loginService;
        }

        public ICommand ImportCommand => new MyLoadingRelayCommand(ImportAsync);

        private async Task ImportAsync()
        {
            var str = await _importExportService.ImportExportFileAsync();
            if (str != null)
                await _folderRepository.ImportDataAsync(str);
        }

        public ICommand ExportCommand => new MyLoadingRelayCommand(ExportAsync);

        private async Task ExportAsync()
        {
            var exportString = await _folderRepository.ExportDataAsync();
            await _importExportService.SaveExportFileAsync(exportString);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => Set(ref _newPassword, value);
        }

        public ICommand ResetApplicationCommand => new MyLoadingRelayCommand(ResetApplicationAsync);

        private async Task ResetApplicationAsync()
        {
            if (await _folderRepository.ClearAllDataAsync())
            {
                await _interactionService.ShowMessageAsync("reset successful, the application will close now");
                _interactionService.CloseApplication();
            }
        }

        public ICommand SetNewPasswordCommand => new MyLoadingRelayCommand(SetNewPasswordAsync());

        private async Task SetNewPasswordAsync()
        {
            var hash = _loginService.HashPassword(NewPassword);
            _loginService.RegisterValidPassword();
            if (await _folderRepository.ClearAllDataAsync())
            {
                await _interactionService.ShowMessageAsync("new password set, the application will close now");
                _interactionService.CloseApplication();
            }
        }

        public ICommand ExportCredentialsCommand => new MyLoadingRelayCommand(ExportCredentialsAsync);

        private async Task ExportCredentialsAsync()
        {
            await _importExportService.SaveCredentialsFileAsync(await _folderRepository.ExportCredentialsAsync());
        }

        public ICommand ImportCredentialsCommand => new MyLoadingRelayCommand(ImportCredentialsAsync);

        private async Task ImportCredentialsAsync()
        {
            if (await _folderRepository.ImportCredentialsAsync(await _importExportService.ImportCredentialsFileAsync()))
            {
                await _interactionService.ShowMessageAsync("import successful, the application will close now");
                _interactionService.CloseApplication();
            }
        }
    }
}
