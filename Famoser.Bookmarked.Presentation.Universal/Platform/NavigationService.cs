﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Famoser.Bookmarked.Presentation.Universal.Entity;
using Famoser.Bookmarked.Presentation.Universal.Pages.Entry.Common;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Services.Interfaces;

namespace Famoser.Bookmarked.Presentation.Universal.Platform
{
    public class NavigationService : INavigationService
    {
        private readonly ConcurrentDictionary<string, Tuple<Type, object>> _pagesByKey = new ConcurrentDictionary<string, Tuple<Type, object>>();
        private readonly ConcurrentStack<Action> _goBackActions = new ConcurrentStack<Action>();

        private string RootPageKey => "-- ROOT--";
        private string UnknownPageKey => "-- UNKNOWN--";

        private void ConfigureBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = CanGoBack() ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
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
                    ev.Handled = GoBack();
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

        public bool CanGoBack()
        {
            return _backEnabled && _goBackActions.Count > 0;
        }

        /// <summary>
        /// If possible, discards the current page and displays the previous page on the navigation stack.
        /// </summary>
        public bool GoBack()
        {
            if (!CanGoBack())
            {
                return false;
            }
            Action res;
            while (!_goBackActions.TryPop(out res))
            {
                if (_goBackActions.Count == 0)
                    return false;
            }
            res.Invoke();
            return true;
        }

        private bool _backEnabled = true;
        public void DisableBack()
        {
            _backEnabled = false;
            ConfigureBackButton();
        }

        public void EnableBack()
        {
            _backEnabled = true;
            ConfigureBackButton();
        }

        public void NavigateTo(string pageKey, bool removeCurrent = false)
        {
            ExecuteOnce();

            if (!_pagesByKey.ContainsKey(pageKey))
                throw new ArgumentException(string.Format("No such page: {0}. Did you forget to call NavigationService.Configure?", pageKey), "pageKey");

            this.EnableBack();
            lock (this)
            {
                var frame = (Frame)Window.Current.Content;
                frame.Navigate(_pagesByKey[pageKey].Item1, _pagesByKey[pageKey].Item2);


                if (removeCurrent)
                {
                    if (frame.BackStack.Count > 0)
                        frame.BackStack.RemoveAt(frame.BackStack.Count - 1);
                }
                else
                {
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
                ConfigureBackButton();
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
            while (!_pagesByKey.TryAdd(key, new Tuple<Type, object>(pageType, navigationParameter))) ;
        }
    }
}
