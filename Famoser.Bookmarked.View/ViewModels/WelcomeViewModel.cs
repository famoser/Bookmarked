using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPasswordService _passwordService;
        private readonly IInteractionService _interactionService;
        private readonly IFolderRepository _folderRepository;
        private readonly ILoginService _loginService;
        private readonly IImportExportService _importExportService;

        public WelcomeViewModel(INavigationService navigationService, IPasswordService passwordService, IInteractionService interactionService, IFolderRepository folderRepository, ILoginService loginService, IImportExportService importExportService)
        {
            _navigationService = navigationService;
            _passwordService = passwordService;
            _interactionService = interactionService;
            _folderRepository = folderRepository;
            _loginService = loginService;
            _importExportService = importExportService;
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private string _confirmationPassword;
        public string ConfirmationPassword
        {
            get => _confirmationPassword;
            set => Set(ref _confirmationPassword, value);
        }

        public ICommand ConfirmPasswordCommand => new MyLoadingRelayCommand(async () =>
        {
            if (Password != ConfirmationPassword)
            {
                await _interactionService.ShowMessageAsync("passwords do not match");
            }
            else
            {
                if (!string.IsNullOrEmpty(Password))
                {
                    var hash = _loginService.HashPassword(Password);
                    await _passwordService.SetPasswordAsync(hash);
                    _navigationService.NavigateTo(PageKeys.Login.ToString(), true);
                }
                else
                {
                    await _interactionService.ShowMessageAsync("no password provided");
                }
            }
        });

        public ICommand ImportCredentialsCommand => new MyLoadingRelayCommand(async () =>
        {
            if (string.IsNullOrEmpty(Password))
            {
                await _interactionService.ShowMessageAsync("password is empty");
            }
            var importFile = await _interactionService.GetFileContentAsync("bmd_cred");
            if (string.IsNullOrEmpty(importFile))
            {
                await _interactionService.ShowMessageAsync("file is empty");
            }

            await _passwordService.SetPasswordAsync(Password);
            if (await _folderRepository.ImportCredentialsAsync(importFile))
            {
                await _interactionService.ShowMessageAsync("import successful!");
                _interactionService.CloseApplication();
            }
            else
            {
                await _interactionService.ShowMessageAsync("import failed; is the password correct?");
            }
        });

    }
}
