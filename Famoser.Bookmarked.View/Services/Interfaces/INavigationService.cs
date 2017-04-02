using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.FrameworkEssentials.View.Interfaces;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface INavigationService
    {
        string RootPageKey { get; }

        string UnknownPageKey { get; }

        string CurrentPageKey { get; }

        void GoBack();

        void NavigateTo(string pageKey, bool removeCurrent = false);

        void FakeNavigation(Action execute);

        void Configure(string key, Type pageType);
    }
}
