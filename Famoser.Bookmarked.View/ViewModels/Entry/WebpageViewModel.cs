using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Helper;
using Famoser.Bookmarked.View.ViewModels.Entry.Abstract;
using Famoser.FrameworkEssentials.Services;
using Famoser.FrameworkEssentials.Services.Interfaces;

namespace Famoser.Bookmarked.View.ViewModels.Entry
{
    public class WebpageViewModel : EntryViewModel<WebpageModel>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IHistoryNavigationService _navigationService;

        public WebpageViewModel(IFolderRepository folderRepository, IHistoryNavigationService navigationService) : base(folderRepository, navigationService)
        {
            _folderRepository = folderRepository;
            _navigationService = navigationService;
        }

        protected override Pages GetViewPage()
        {
            return Pages.ViewWebpage;
        }

        protected override Pages GetEditPage()
        {
            return Pages.EditWebpage;
        }

        protected override async void SelectedEntryContentOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName ==
                ReflectionHelper.GetPropertyName(() => SelectedEntryContent.WebpageUrl))
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
    }
}
