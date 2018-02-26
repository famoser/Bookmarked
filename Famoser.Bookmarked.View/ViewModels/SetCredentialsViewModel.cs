using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class SetCredentialsViewModel : BaseViewModel
    {
        private readonly IInteractionService _interactionService;
        private readonly IFolderRepository _folderRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILoginService _loginService;

        private string _password;

        public SetCredentialsViewModel(IInteractionService interactionService, IPasswordService passwordService, IFolderRepository folderRepository, ILoginService loginService)
        {
            _interactionService = interactionService;
            _passwordService = passwordService;
            _folderRepository = folderRepository;
            _loginService = loginService;
        }

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public ICommand ImportCredentialsCommand => new MyLoadingRelayCommand(async () =>
        {
            if (string.IsNullOrEmpty(Password))
            {
                await _interactionService.ShowMessageAsync("password is empty");
                return;
            }
            var importFile = await _interactionService.GetFileContentAsync("bmd_cred");
            if (string.IsNullOrEmpty(importFile))
            {
                await _interactionService.ShowMessageAsync("file is empty");
                return;
            }

            var hash = _loginService.HashPassword(Password);
            if (await _folderRepository.ImportCredentialsAsync(importFile, hash))
            {
                await _passwordService.SetPasswordAsync(hash);
                await _interactionService.ShowMessageAsync("import successful; the application will close now");
                _interactionService.CloseApplication();
            }
            else
            {
                await _interactionService.ShowMessageAsync("import failed; is the password correct?");
            }
        });
    }
}
