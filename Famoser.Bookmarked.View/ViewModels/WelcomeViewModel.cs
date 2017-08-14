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

        public WelcomeViewModel(INavigationService navigationService, IPasswordService passwordService, IInteractionService interactionService, IFolderRepository folderRepository, ILoginService loginService)
        {
            _navigationService = navigationService;
            _passwordService = passwordService;
            _interactionService = interactionService;
            _folderRepository = folderRepository;
            _loginService = loginService;
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

        public ICommand SetPasswordCommand => new MyLoadingRelayCommand(async () =>
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

        public ICommand CreateFromExistingFile => new MyLoadingRelayCommand<Tuple<string, string>>(async (s) =>
        {
            if (string.IsNullOrEmpty(s.Item1))
            {
                await _interactionService.ShowMessageAsync("password is empty");
                return;
            }
            if (string.IsNullOrEmpty(s.Item2))
            {
                await _interactionService.ShowMessageAsync("file is empty");
                return;
            }

            await _passwordService.SetPasswordAsync(s.Item1);
            if (await _folderRepository.ImportCredentialsAsync(s.Item2))
            {
                await _interactionService.ShowMessageAsync("import successful!");
                _navigationService.GoBack();
            }
            else
            {
                await _interactionService.ShowMessageAsync("import failed; is the password correct?");
            }
        });

    }
}
