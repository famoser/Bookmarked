using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Extensions;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using GalaSoft.MvvmLight.Command;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class GeneratePasswordViewModel : BaseViewModel
    {
        private readonly IPasswordService _passwordService;
        private readonly IInteractionService _interactionService;

        public GeneratePasswordViewModel(IPasswordService passwordService, IInteractionService interactionService)
        {
            _passwordService = passwordService;
            _interactionService = interactionService;
            _passwordTypes = new ObservableCollection<ValueWrapper<PasswordType>>(System.Enum.GetValues(typeof(PasswordType)).Cast<PasswordType>().Select(e => new ValueWrapper<PasswordType>(e)));
            _selectedPasswordType = PasswordTypes.FirstOrDefault();
        }

        private string _password;
        public string Password
        {
            get => _password;
            private set => Set(ref _password, value);
        }

        private bool _copyToClipboardEnabled;
        public bool CopyToClipboardEnabled
        {
            get => _copyToClipboardEnabled;
            set
            {
                Set(ref _copyToClipboardEnabled, value);
                if (CopyToClipboardEnabled)
                {
                    GeneratePasswordCommand.Execute(null);
                }
            }
        }

        private int _passwordLength = 16;
        public int PasswordLength
        {
            get => _passwordLength;
            set
            {
                if (Set(ref _passwordLength, value))
                {
                    GeneratePasswordCommand.Execute(null);
                }
            }
        }

        private ValueWrapper<PasswordType> _selectedPasswordType;
        public ValueWrapper<PasswordType> SelectedPasswordType
        {
            get => _selectedPasswordType;
            set
            {
                if (Set(ref _selectedPasswordType, value))
                {
                    GeneratePasswordCommand.Execute(null);
                }
            }
        }

        public ICommand GeneratePasswordCommand => new RelayCommand(() =>
        {
            if (SelectedPasswordType != null)
            {
                Password = _passwordService.GeneratePassword(SelectedPasswordType.Value, PasswordLength);
                if (CopyToClipboardEnabled)
                {
                    _interactionService.CopyToClipboard(Password);
                }
            }
        });

        private ObservableCollection<ValueWrapper<PasswordType>> _passwordTypes;
        public ObservableCollection<ValueWrapper<PasswordType>> PasswordTypes
        {
            get => _passwordTypes;
            private set => Set(ref _passwordTypes, value);
        }
    }
}
