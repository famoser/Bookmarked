﻿using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Folder.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;

namespace Famoser.Bookmarked.View.ViewModels.Folder
{
    public class EditFolderViewModel : FolderViewModel
    {
        public EditFolderViewModel(IFolderRepository folderRepository, IHistoryNavigationService navigationService) : base(folderRepository, navigationService)
        {
        }

        public ICommand SaveEntryCommand => new LoadingRelayCommand(async () =>
        {
            await _folderRepository.SaveFolderAsync(Folder);
            _navigationService.GoBack();
        });

        public ICommand DeleteEntryCommand => new LoadingRelayCommand(async () =>
        {
            await _folderRepository.RemoveFolderAsync(Folder);
            _navigationService.GoBack();
            _navigationService.GoBack();
        });
    }
}
