using System;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface INavigationService
    {
        bool CanGoBack();

        bool GoBack();

        void NavigateTo(string pageKey, bool removeCurrent = false);

        void FakeNavigation(Action execute);
    }
}
