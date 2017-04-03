using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Presentation.Universal.Entity;
using Famoser.Bookmarked.Presentation.Universal.Pages;
using Famoser.Bookmarked.Presentation.Universal.Pages.Entry.Webpage;
using Famoser.Bookmarked.Presentation.Universal.Pages.Folder;
using Famoser.Bookmarked.Presentation.Universal.Platform;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Helper;
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
            SimpleIoc.Default.Register<INavigationService>(ConstructNavigationService);
            SimpleIoc.Default.Register<IInteractionService, InteractionService>();

            //ClearAll();
        }

        private static async Task ClearAll()
        {
            var files = await ApplicationData.Current.LocalCacheFolder.GetFilesAsync();
            foreach (var storageFile in files)
            {
                await storageFile.DeleteAsync();
            }

            files = await ApplicationData.Current.RoamingFolder.GetFilesAsync();
            foreach (var storageFile in files)
            {
                await storageFile.DeleteAsync();
            }
        }

        private static NavigationService ConstructNavigationService()
        {
            var ngs = new NavigationService();
            //add login & mainpage
            ngs.Configure(PageKeys.Login.ToString(), typeof(LoginPage));
            ngs.Configure(PageKeys.Navigation.ToString(), typeof(NavigationPage));

            //add folder pages
            ngs.Configure(PageKeys.AddFolder.ToString(), typeof(AddFolderPage));
            ngs.Configure(PageKeys.EditFolder.ToString(), typeof(EditFolderPage));

            //entry lookup (connects entries with their View & Edit frame)
            var lookup = new Dictionary<ContentType, Tuple<Type, Type>>()
            {
                {ContentType.Webpage, new Tuple<Type, Type>(typeof(ViewWebpage), typeof(EditWebpage))}
            };

            //add pages of entries
            foreach (var ctm in ContentHelper.GetContentTypeModels())
            {
                if (lookup.ContainsKey(ctm.ContentType))
                {
                    var parameter = new NavigationParameter()
                    {
                        Name = ctm.Name,
                        ViewModelType = ctm.ViewModelType,
                        ViewFrameType = lookup[ctm.ContentType].Item1,
                        EditFrameType = lookup[ctm.ContentType].Item2
                    };
                    ngs.AddEntryNavigation(ctm.AddPageKey, ctm.EditPageKey, ctm.ViewPageKey, parameter);
                }
            }

            return ngs;
        }
    }
}
