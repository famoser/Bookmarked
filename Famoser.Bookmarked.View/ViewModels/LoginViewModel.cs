using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPasswordService _passwordService;
        private readonly IInteractionService _interactionService;

        public LoginViewModel(INavigationService navigationService, IPasswordService passwordService, IInteractionService interactionService)
        {
            _navigationService = navigationService;
            _passwordService = passwordService;
            _interactionService = interactionService;

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            IsFirstTime = await _passwordService.CheckIsFirstTimeAsync();
            if (IsFirstTime)
            {
                Message = "Welcome! Choose a password";
                ShowMessage = true;
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

        public ICommand LoginCommand => new LoadingRelayCommand(async () =>
        {
            if (IsFirstTime && Password != ConfirmationPassword)
            {
                FlashMessage("Passwords do not match");
                Password = "";
                ConfirmationPassword = "";
                return;
            }

            var hash = _interactionService.HashPassword(Password);
            if (await _passwordService.TryPasswordAsync(hash))
            {
                _navigationService.NavigateTo(PageKeys.Navigation.ToString(), true);
                IsFirstTime = false;
            }
            else
            {
                FlashMessage("Password wrong");
            }
        });

        public ICommand HelpCommand => new LoadingRelayCommand(() => _navigationService.NavigateTo(PageKeys.Help.ToString()));
    }
}
