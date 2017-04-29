﻿using System.Windows.Input;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Command;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Folder.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Commands;

namespace Famoser.Bookmarked.View.ViewModels.Folder
{
    public class EditFolderViewModel : FolderViewModel
    {
        public EditFolderViewModel(IFolderRepository folderRepository, INavigationService navigationService) : base(folderRepository, navigationService)
        {
        }

        public ICommand SaveFolderCommand => new MyLoadingRelayCommand(async () =>
        {
            await _folderRepository.SaveFolderAsync(Folder);
            _navigationService.GoBack();
        });

        public ICommand RemoveFolderCommand => new MyLoadingRelayCommand(async () =>
        {
            await _folderRepository.MoveFolderToGarbageAsync(Folder);
            _navigationService.GoBack();
            _navigationService.GoBack();
        });
    }
}
