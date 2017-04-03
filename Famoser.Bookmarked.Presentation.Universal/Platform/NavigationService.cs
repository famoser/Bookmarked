﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Famoser.Bookmarked.Presentation.Universal.Entity;
using Famoser.Bookmarked.Presentation.Universal.Pages.Entry.Webpage;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.FrameworkEssentials.View.Interfaces;
using AddEntryPage = Famoser.Bookmarked.Presentation.Universal.Pages.Entry.Common.AddEntryPage;
using EditEntryPage = Famoser.Bookmarked.Presentation.Universal.Pages.Entry.Common.EditEntryPage;
using ViewEntryPage = Famoser.Bookmarked.Presentation.Universal.Pages.Entry.Common.ViewEntryPage;

namespace Famoser.Bookmarked.Presentation.Universal.Platform
{
    class NavigationService : INavigationService
    {
        private readonly ConcurrentDictionary<string, Tuple<Type, object>> _pagesByKey = new ConcurrentDictionary<string, Tuple<Type, object>>();
        private readonly ConcurrentStack<Action> _goBackActions = new ConcurrentStack<Action>();

        public string RootPageKey => "-- ROOT--";
        public string UnknownPageKey => "-- UNKNOWN--";

        private void ConfigureBackButton()
        {
            if (_goBackActions.Count < 1)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
        }

        private bool _executed;
        private void ExecuteOnce()
        {
            lock (this)
            {
                if (_executed)
                    return;

                _executed = true;
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, ev) =>
            {
                if (!ev.Handled)
                {
                    GoBack();
                    ev.Handled = true;
                }
            };

            ConfigureBackButton();
        }

        /// <summary>
        /// The key corresponding to the currently displayed page.
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                Frame frame = (Frame)Window.Current.Content;
                if (frame.BackStackDepth == 0)
                    return RootPageKey;
                if (frame.Content == null)
                    return UnknownPageKey;
                Type currentType = frame.Content.GetType();
                if (_pagesByKey.All(p => p.Value.Item1 != currentType))
                    return UnknownPageKey;
                return _pagesByKey.FirstOrDefault(i => i.Value.Item1 == currentType).Key;
            }
        }

        /// <summary>
        /// If possible, discards the current page and displays the previous page on the navigation stack.
        /// </summary>
        public void GoBack()
        {
            Action res;
            while (!_goBackActions.TryPop(out res)) { }
            res.Invoke();
        }

        public void NavigateTo(string pageKey, bool removeCurrent = false)
        {
            ExecuteOnce();

            if (!_pagesByKey.ContainsKey(pageKey))
                throw new ArgumentException(string.Format("No such page: {0}. Did you forget to call NavigationService.Configure?", pageKey), "pageKey");

            lock (this)
            {
                var frame = (Frame)Window.Current.Content;
                frame.Navigate(_pagesByKey[pageKey].Item1, _pagesByKey[pageKey].Item2);

                if (removeCurrent)
                    frame.BackStack.RemoveAt(frame.BackStack.Count - 1);

                ConfigureBackButton();


                if (!removeCurrent)
                    //add go back action
                    _goBackActions.Push(() =>
                    {
                        if (!frame.CanGoBack)
                        {
                            Application.Current.Exit();
                        }
                        else
                        {
                            frame.GoBack();
                            ConfigureBackButton();
                        }
                    });
            }
        }

        public void FakeNavigation(Action execute)
        {
            //add go back action
            _goBackActions.Push(() =>
            {
                execute.Invoke();
                ConfigureBackButton();
            });
            ConfigureBackButton();
        }


        public void AddEntryNavigation(PageKeys addPage, PageKeys editPage, PageKeys viewPage, NavigationParameter parameter)
        {
            Configure(addPage.ToString(), typeof(AddEntryPage), parameter);
            Configure(editPage.ToString(), typeof(EditEntryPage), parameter);
            Configure(viewPage.ToString(), typeof(ViewEntryPage), parameter);
        }

        /// <summary>
        /// Adds a key/page pair to the navigation service.
        /// </summary>
        public void Configure(string key, Type pageType, object navigationParameter = null)
        {
            if (_pagesByKey.ContainsKey(key))
                throw new ArgumentException("This key is already used: " + key);
            if (_pagesByKey.Any(p => p.Value.Item1 == pageType))
                throw new ArgumentException("This type is already configured with key " + _pagesByKey.First(p => p.Value.Item1 == pageType).Key);
            while (!_pagesByKey.TryAdd(key, new Tuple<Type, object>(pageType, navigationParameter))) ;
        }
    }
}
