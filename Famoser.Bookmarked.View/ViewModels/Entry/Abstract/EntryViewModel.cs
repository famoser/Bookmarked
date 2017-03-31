﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries.Base;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;

namespace Famoser.Bookmarked.View.ViewModels.Entry.Abstract
{
    public abstract class EntryViewModel<T> : BaseViewModel where T : ContentModel, new()
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IHistoryNavigationService _navigationService;
        private readonly Stack<FolderModel> _folderHistory = new Stack<FolderModel>();

        public EntryViewModel(IFolderRepository folderRepository, IHistoryNavigationService navigationService)
        {
            _folderRepository = folderRepository;
            _navigationService = navigationService;
        }

        internal void SetEntry(EntryModel model, CrudState state)
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

        public ICommand SaveEntryCommand => new LoadingRelayCommand(async () =>
        {
            if (_state == CrudState.Edit)
            {
                _navigationService.GoBack();
                SetCrudState(CrudState.View);
            }
            if (_state == CrudState.Add)
            {
                _navigationService.GoBack();
                _navigationService.NavigateTo(GetViewPage().ToString());
                SetCrudState(CrudState.View);
            }
            await _folderRepository.SaveEntryAsync(SelectedEntry, SelectedEntryContent);
        });

        public ICommand EditEntryCommand => new LoadingRelayCommand(() =>
        {
            _navigationService.NavigateTo(GetEditPage().ToString());
            SetCrudState(CrudState.Edit);
        });

        public ICommand AbortCommand => new LoadingRelayCommand(() =>
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

        public ICommand RemoveEntryCommand => new LoadingRelayCommand(async () =>
        {
            await _folderRepository.RemoveEntryAsync(SelectedEntry);
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
        
        protected abstract Pages GetViewPage();
        protected abstract Pages GetEditPage();
    }
}