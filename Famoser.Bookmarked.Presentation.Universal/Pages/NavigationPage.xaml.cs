using Windows.Foundation.Metadata;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.View.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Famoser.Bookmarked.Presentation.Universal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            this.InitializeComponent();
        }

        private NavigationViewModel ViewModel => DataContext as NavigationViewModel;

        private static bool _firstTime = true;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_firstTime)
                return;

            _firstTime = false;


            if (ViewModel.RefreshCommand.CanExecute(null))
                ViewModel.RefreshCommand.Execute(null);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is FolderModel folder)
            {
                if (ViewModel.SelectFolderCommand.CanExecute(folder))
                {
                    ViewModel.SelectFolderCommand.Execute(folder);
                }
            }
            if (e.ClickedItem is EntryModel entry)
            {
                if (ViewModel.SelectEntryCommand.CanExecute(entry))
                {
                    ViewModel.SelectEntryCommand.Execute(entry);
                }
            }
        }

        private void UIElement_OnTapped(object sender = null, TappedRoutedEventArgs e = null)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
    }
}
