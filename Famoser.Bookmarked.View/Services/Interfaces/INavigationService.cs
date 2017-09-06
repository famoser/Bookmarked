using System;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface INavigationService
    {
        bool CanGoBack();

        bool GoBack();

        void DisableBack();

        void EnableBack();

        void NavigateTo(string pageKey, bool removeCurrent = false);

        void FakeNavigation(Action execute);
    }
}
