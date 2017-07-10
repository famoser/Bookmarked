using System.ComponentModel;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.Helper;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Entry.Abstract;

namespace Famoser.Bookmarked.View.ViewModels.Entry.Base
{
    public abstract class WithUrlViewModel<T> : EntryViewModel<T> where T : WebpageModel, new()
    {
        public WithUrlViewModel(IFolderRepository folderRepository, INavigationService navigationService, IApiService apiService) : base(folderRepository, navigationService, apiService)
        {
        }

        protected override async void SelectedEntryContentOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            base.SelectedEntryContentOnPropertyChanged(sender, propertyChangedEventArgs);
            if (propertyChangedEventArgs.PropertyName == ReflectionHelper.GetPropertyName(() => SelectedEntryContent.WebpageUrl))
            {
                await SourceUriChangedAsync(SelectedEntryContent.WebpageUrl);
            }
        }
    }
}
