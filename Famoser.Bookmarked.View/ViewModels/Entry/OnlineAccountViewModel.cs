using System.ComponentModel;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Helper;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Entry.Base;
using GalaSoft.MvvmLight.Command;

namespace Famoser.Bookmarked.View.ViewModels.Entry
{
    public class OnlineAccountViewModel : WithUrlViewModel<OnlineAccountModel>
    {
        private IInteractionService _interactionService;
        public OnlineAccountViewModel(IFolderRepository folderRepository, INavigationService navigationService, IApiService apiService, IInteractionService interactionService) : base(folderRepository, navigationService, apiService)
        {
            _interactionService = interactionService;

            CopyPasswordToClipboard = new RelayCommand(() => _interactionService.CopyToClipboard(SelectedEntryContent.Password));
            CopyUsernameToClipboard = new RelayCommand(() => _interactionService.CopyToClipboard(SelectedEntryContent.UserName));
        }
        public ICommand CopyPasswordToClipboard { get; }
        public ICommand CopyUsernameToClipboard { get; }

        protected override void SelectedEntryContentOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            base.SelectedEntryContentOnPropertyChanged(sender, propertyChangedEventArgs);
            if (propertyChangedEventArgs.PropertyName == ReflectionHelper.GetPropertyName(() => SelectedEntryContent.UserName))
            {
                if (SelectedEntryContent.UserName.Contains("@") && string.IsNullOrEmpty(SelectedEntryContent.Email))
                    SelectedEntryContent.Email = SelectedEntryContent.UserName;
            }
        }
    }
}
