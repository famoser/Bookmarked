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
        private IFolderRepository _folderRepository;
        private IApiService _apiService;
        private IInteractionService _interactionService;

        public ImportViewModel(IFolderRepository folderRepository, IApiService apiService, IInteractionService interactionService)
        {
            _folderRepository = folderRepository;
            _apiService = apiService;
            _interactionService = interactionService;
        }

        public ICommand ImportCommand => new MyLoadingRelayCommand(ImportAsync);

        private async Task ImportAsync()
        {
            var str = await _interactionService.ImportExportFileAsync();
            if (str != null)
                await _folderRepository.ImportDataAsync(str);
        }

        public ICommand ExportCommand => new MyLoadingRelayCommand(ExportAsync);

        private async Task ExportAsync()
        {
            var exportString = await _folderRepository.ExportDataAsync();
            await _interactionService.SaveExportFileAsync(exportString);
        }

        public ICommand ResetApplicationCommand => new MyLoadingRelayCommand(ResetApplicationAsync);

        private async Task ResetApplicationAsync()
        {
            if (await _folderRepository.ClearAllDataAsync())
            {
                _interactionService.CloseApplication();
            }
        }

        public ICommand ExportCredentialsCommand => new MyLoadingRelayCommand(ExportCredentialsAsync);

        private async Task ExportCredentialsAsync()
        {
            await _interactionService.SaveCredentialsFileAsync(await _folderRepository.ExportCredentialsAsync());
        }

        public ICommand ImportCredentialsCommand => new MyLoadingRelayCommand(ImportCredentialsAsync);

        private async Task ImportCredentialsAsync()
        {
            if (await _folderRepository.ImportCredentialsAsync(await _interactionService.ImportCredentialsFileAsync()))
            {
                _interactionService.CloseApplication();
            }
        }
    }
}
