using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Services.Interfaces;

namespace Famoser.Bookmarked.View.ViewModels.Base
{
    public class SetCredentialsViewModel : BaseViewModel
    {
        private readonly IInteractionService _interactionService;
        private readonly IFolderRepository _folderRepository;
        private readonly IPasswordService _passwordService;

        private string _password;

        public SetCredentialsViewModel(IInteractionService interactionService, IPasswordService passwordService, IFolderRepository folderRepository)
        {
            _interactionService = interactionService;
            _passwordService = passwordService;
            _folderRepository = folderRepository;
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

            if (await _folderRepository.ImportCredentialsAsync(importFile, Password))
            {
                await _passwordService.SetPasswordAsync(Password);
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
