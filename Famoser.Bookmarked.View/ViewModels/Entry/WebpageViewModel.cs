using System;
using System.ComponentModel;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Helper;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Entry.Abstract;
using Famoser.FrameworkEssentials.Services;
using Famoser.FrameworkEssentials.Services.Interfaces;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.View.ViewModels.Entry
{
    public class WebpageViewModel : EntryViewModel<WebpageModel>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly INavigationService _navigationService;

        public WebpageViewModel(IFolderRepository folderRepository, INavigationService navigationService) : base(folderRepository, navigationService)
        {
            _folderRepository = folderRepository;
            _navigationService = navigationService;
        }

        protected override async void SelectedEntryContentOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == ReflectionHelper.GetPropertyName(() => SelectedEntryContent.WebpageUrl))
            {
                try
                {
                    var service = new HttpService();
                    var resp = await service.DownloadAsync(new Uri(SelectedEntryContent.WebpageUrl, "favicon.ico"));
                    SelectedEntryContent.Icon = await resp.GetResponseAsByteArrayAsync();
                }
                catch
                {
                    //swallow cause it does not really matter
                }
            }
        }

        protected override ContentTypeModel GetContentTypeModel()
        {
            return ContentHelper.GetWebpageContentTypeModel();
        }
    }
}
