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
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        private bool _passwordUnlockFailed;
        public bool PasswordUnlockFailed
        {
            get { return _passwordUnlockFailed; }
            set { Set(ref _passwordUnlockFailed, value); }
        }

        private bool _isFirstTime;
        public bool IsFirstTime
        {
            get { return _isFirstTime; }
            set { Set(ref _isFirstTime, value); }
        }
        
        public ICommand LoginCommand => new LoadingRelayCommand(async () =>
        {
            var hash = _interactionService.HashPassword(Password);
            if (await _passwordService.TryPasswordAsync(hash))
            {
                _navigationService.NavigateTo(Pages.Navigation.ToString(), true);
                IsFirstTime = false;
            }
            else
            {
                PasswordUnlockFailed = true;
                await Task.Delay(TimeSpan.FromSeconds(2));
                PasswordUnlockFailed = false;
            }
        });
    }
}
