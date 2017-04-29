using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries.Base;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Helper;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.Bookmarked.View.ViewModels.Interface;
using Famoser.FrameworkEssentials.Services;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;

namespace Famoser.Bookmarked.View.ViewModels.Entry.Abstract
{
    public abstract class EntryViewModel<T> : BaseViewModel, IEntryViewModel where T : ContentModel, new()
    {
        private readonly IFolderRepository _folderRepository;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly Stack<FolderModel> _folderHistory = new Stack<FolderModel>();

        protected EntryViewModel(IFolderRepository folderRepository, INavigationService navigationService, IApiService apiService)
        {
            _folderRepository = folderRepository;
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public void SetEntry(EntryModel model, CrudState state)
        {
            if (SelectedEntry != null)
                SelectedEntry.PropertyChanged -= SelectedEntryOnPropertyChanged;
            SelectedEntry = model;
            SelectedEntry.PropertyChanged += SelectedEntryOnPropertyChanged;

            if (SelectedEntryContent != null)
                SelectedEntryContent.PropertyChanged -= SelectedEntryContentOnPropertyChanged;
            SelectedEntryContent = _folderRepository.GetEntryContent<T>(model);
            SelectedEntryContent.PropertyChanged += SelectedEntryContentOnPropertyChanged;

            _state = state;
        }

        protected virtual void SelectedEntryOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {

        }

        protected virtual void SelectedEntryContentOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {

        }

        private CrudState _state;

        private void SetCrudState(CrudState state)
        {
            _state = state;
        }

        private EntryModel _selectedEntry;
        public EntryModel SelectedEntry
        {
            get { return _selectedEntry; }
            private set { Set(ref _selectedEntry, value); }
        }

        private T _selectedEntryContent;
        public T SelectedEntryContent
        {
            get { return _selectedEntryContent; }
            private set { Set(ref _selectedEntryContent, value); }
        }

        public ICommand SaveEntryCommand => new MyLoadingRelayCommand(async () =>
        {
            if (_state == CrudState.Edit)
            {
                _navigationService.GoBack();
                SetCrudState(CrudState.View);
            }
            if (_state == CrudState.Add)
            {
                _navigationService.NavigateTo(GetContentTypeModel().ViewPageKey.ToString(), true);
                SetCrudState(CrudState.View);
            }
            await _folderRepository.SaveEntryAsync(SelectedEntry, SelectedEntryContent);
        });

        public ICommand EditEntryCommand => new MyLoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(GetContentTypeModel().EditPageKey.ToString());
            SetCrudState(CrudState.Edit);
        });

        public ICommand AbortCommand => new MyLoadingRelayCommand(() =>
        {
            if (_state == CrudState.Edit)
            {
                //resets vm
                 SetEntry(SelectedEntry, CrudState.View);
                _navigationService.GoBack();
            }
            else if (_state == CrudState.Add)
            {
                _navigationService.GoBack();
            }
        });

        public ICommand RemoveEntryCommand => new MyLoadingRelayCommand(async () =>
        {
            await _folderRepository.MoveEntryToGarbageAsync(SelectedEntry);
            if (_state == CrudState.Edit)
            {
                //go back to View and then go back for real
                _navigationService.GoBack();
                _navigationService.GoBack();
            }
            else
            {
                _navigationService.GoBack();
            }
        });

        private ContentTypeModel GetContentTypeModel()
        {
            return ContentHelper.GetContentTypeModel(typeof(T));
        }

        protected async Task SetIconUri(Uri uri)
        {
            try
            {
                SelectedEntry.IconUri = await _apiService.GetIconUriAsync(uri);
            }
            catch
            {
                //swallow cause it does not really matter
            }
        }
    }
}
