using System;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface INavigationService
    {
        string RootPageKey { get; }

        string UnknownPageKey { get; }

        string CurrentPageKey { get; }

        bool GoBack();

        void NavigateTo(string pageKey, bool removeCurrent = false);

        void FakeNavigation(Action execute);
    }
}
