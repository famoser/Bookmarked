using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class ExchangeViewModel : SetCredentialsViewModel
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IInteractionService _interactionService;

        public ExchangeViewModel(IFolderRepository folderRepository, IInteractionService interactionService, IPasswordService passwordService)
            : base(interactionService,  passwordService, folderRepository)
        {
            _folderRepository = folderRepository;
            _interactionService = interactionService;
        }

        public ICommand ImportDataCommand => new MyLoadingRelayCommand(ImportAsync);

        private async Task ImportAsync()
        {
            var str = await _interactionService.GetFileContentAsync("bmd_data");
            if (str != null)
                await _folderRepository.ImportDataAsync(str);
        }

        public ICommand ExportDataCommand => new MyLoadingRelayCommand(ExportAsync);

        private async Task ExportAsync()
        {
            var exportString = await _folderRepository.ExportDataAsync();
            await _interactionService.SaveFileAsync(exportString, "csv", "bookmarked_export");
        }

        public ICommand SaveImportTemplateCommand => new MyLoadingRelayCommand(SaveImportTemplateAsync);

        private async Task SaveImportTemplateAsync()
        {
            var exportString = await _folderRepository.GetImportDataTemplateAsync();
            await _interactionService.SaveFileAsync(exportString, "csv", "bookmarked_import_template");
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
        }
    }
}
