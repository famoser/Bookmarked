using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Famoser.Bookmarked.View.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Famoser.Bookmarked.Presentation.Universal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
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

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var vm = DataContext as LoginViewModel;
            vm?.BootAsync();
        }
    }
}
