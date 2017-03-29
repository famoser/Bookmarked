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
using Famoser.Bookmarked.View.ViewModels.Folder.Base;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Famoser.Bookmarked.Presentation.Universal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // If we have a phone contract, hide the status bar
            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                var statusBar = StatusBar.GetForCurrentView();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                statusBar.HideAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private FolderViewModel ViewModel => DataContext as FolderViewModel;

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
        }

        private void UIElement_OnTapped(object sender = null, TappedRoutedEventArgs e = null)
        {
            LocationSplitView.IsPaneOpen = !LocationSplitView.IsPaneOpen;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            UIElement_OnTapped();
            WeekDayGrid.Visibility = Visibility.Visible;
            CoursesGrid.Visibility = Visibility.Collapsed;
        }

        private string HashPassword(string password)
        {
            // put the string in a buffer
            IBuffer input = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);

            // hash it with SHA 256
            var hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            IBuffer hashed = hasher.HashData(input);

            // return as base64
            return CryptographicBuffer.EncodeToBase64String(hashed);
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UIElement_OnTapped();
            WeekDayGrid.Visibility = Visibility.Collapsed;
            CoursesGrid.Visibility = Visibility.Visible;
        }
    }
}
