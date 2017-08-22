using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class WelcomeViewModel : SetCredentialsViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPasswordService _passwordService;
        private readonly IInteractionService _interactionService;
        private readonly ILoginService _loginService;

        public WelcomeViewModel(INavigationService navigationService, IPasswordService passwordService, IInteractionService interactionService, IFolderRepository folderRepository, ILoginService loginService)
            : base(interactionService, passwordService, folderRepository, loginService)
        {
            _navigationService = navigationService;
            _passwordService = passwordService;
            _interactionService = interactionService;
            _loginService = loginService;
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
    }
}
