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
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPasswordService _passwordService;
        private readonly IInteractionService _interactionService;
        private readonly IFolderRepository _folderRepository;
        private readonly ILoginService _loginService;

        public LoginViewModel(INavigationService navigationService, IPasswordService passwordService, IInteractionService interactionService, IFolderRepository folderRepository, ILoginService loginService)
        {
            _navigationService = navigationService;
            _passwordService = passwordService;
            _interactionService = interactionService;
            _folderRepository = folderRepository;
            _loginService = loginService;
        }

        public async void BootAsync()
        {
            if (await _passwordService.CheckIsFirstTimeAsync())
            {
                _navigationService.NavigateTo(PageKeys.Welcome.ToString(), true);
                return;
            }

            //initialize data
            var rf = _folderRepository.GetRootFolder();
            var sync = await _folderRepository.SyncAsync();

            var hash = await _loginService.TryAlternativeLogin();
            if (hash != null)
            {
                if (await _passwordService.TryPasswordAsync(hash))
                {
                    _loginService.RegisterValidPassword(hash);
                    _navigationService.NavigateTo(PageKeys.Navigation.ToString(), true);
                }
                else
                {
                    _loginService.InvalidateAlternativeLogin();
                }
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public ICommand LoginCommand => new MyLoadingRelayCommand(async () =>
        {
            var hash = _loginService.HashPassword(Password);
            if (await _passwordService.TryPasswordAsync(hash))
            {
                _loginService.RegisterValidPassword(hash);
                _navigationService.NavigateTo(PageKeys.Navigation.ToString(), true);
            }
            else
            {
                await _interactionService.ShowMessageAsync("password wrong");
            }
        });
    }
}
