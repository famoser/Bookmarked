using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IHistoryNavigationService _navigationService;
        private readonly IPasswordService _passwordService;
        private readonly Stack<FolderModel> _folderHistory = new Stack<FolderModel>();

        public LoginViewModel(IHistoryNavigationService navigationService, IPasswordService passwordService)
        {
            _navigationService = navigationService;
            _passwordService = passwordService;
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


        public ICommand LoginCommand => new LoadingRelayCommand(async () =>
        {
            if (await _passwordService.TryPassword(Password))
            {
                _navigationService.NavigateTo(Pages.ViewFolder.ToString());
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
