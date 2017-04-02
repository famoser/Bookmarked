﻿using Famoser.Bookmarked.Presentation.Universal.Pages;
using Famoser.Bookmarked.Presentation.Universal.Pages.Entry.Webpage;
using Famoser.Bookmarked.Presentation.Universal.Platform;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.UniversalWindows.Platform;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.Presentation.Universal.ViewModels
{
    public class ViewModelLocator : BaseViewModelLocator
    {
        static ViewModelLocator()
        {
            SimpleIoc.Default.Register<IStorageService>(() => new StorageService());
            SimpleIoc.Default.Register<IHistoryNavigationService>(ConstructNavigationService);
            SimpleIoc.Default.Register<IInteractionService, InteractionService>();
        }

        private static HistoryNavigationService ConstructNavigationService()
        {
            var ngs = new HistoryNavigationService();
            ngs.Configure(View.Enum.Pages.Login.ToString(), typeof(LoginPage));
            ngs.Configure(View.Enum.Pages.Navigation.ToString(), typeof(NavigationPage));
            ngs.Configure(View.Enum.Pages.ViewWebpage.ToString(), typeof(ViewWebpagePage));
            ngs.Configure(View.Enum.Pages.EditWebpage.ToString(), typeof(EditWebpagePage));
            ngs.Configure(View.Enum.Pages.AddWebpage.ToString(), typeof(AddWebpagePage));
            ngs.Configure(View.Enum.Pages.AddFolder.ToString(), typeof(AddFolderPage));
            ngs.Configure(View.Enum.Pages.EditFolder.ToString(), typeof(EditFolderPage));
            return ngs;
        }
    }
}
