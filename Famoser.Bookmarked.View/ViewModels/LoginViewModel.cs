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

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            IsFirstTime = await _passwordService.CheckIsFirstTimeAsync();
            if (IsFirstTime)
            {
                Message = "Welcome! Choose a password.";
                ShowMessage = true;
            }
            var sync = await _folderRepository.SyncAsync();
            var rf = _folderRepository.GetRootFolder();
            if (rf.Folders.Count > 0 || rf.Entries.Count > 0)
            {
                IsFirstTime = false;
                ShowMessage = false;
                var str = await _loginService.TryAlternativeLogin();
                if (str != null)
                {
                    TryLoginWithHash(str);
                }
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        private string _confirmationPassword;
        public string ConfirmationPassword
        {
            get { return _confirmationPassword; }
            set { Set(ref _confirmationPassword, value); }
        }

        private bool _showMessage;
        public bool ShowMessage
        {
            get { return _showMessage; }
            set { Set(ref _showMessage, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
        }

        private bool _isFirstTime;
        public bool IsFirstTime
        {
            get { return _isFirstTime; }
            set { Set(ref _isFirstTime, value); }
        }

        private async void FlashMessage(string message)
        {
            Message = message;
            ShowMessage = true;
            await Task.Delay(TimeSpan.FromSeconds(2));
            ShowMessage = false;
        }

        public ICommand LoginCommand => new MyLoadingRelayCommand(() =>
        {
            if (IsFirstTime && Password != ConfirmationPassword)
            {
                FlashMessage("passwords do not match");
                Password = "";
                ConfirmationPassword = "";
                return;
            }

            if (!string.IsNullOrEmpty(Password))
            {
                var hash = _loginService.HashPassword(Password);
                TryLoginWithHash(hash);
            }
            else
            {
                FlashMessage("no password");
            }
        });

        private async void TryLoginWithHash(string hash)
        {
            if (await _passwordService.TryPasswordAsync(hash))
            {
                _loginService.RegisterValidPassword(hash);
                await _passwordService.SetPasswordAsync(hash);
                _navigationService.NavigateTo(PageKeys.Navigation.ToString(), true);
                IsFirstTime = false;
            }
            else
            {
                FlashMessage("password wrong");
            }
        }

        public ICommand HelpCommand => new MyLoadingRelayCommand(() => _navigationService.NavigateTo(PageKeys.Info.ToString()));
    }
}
