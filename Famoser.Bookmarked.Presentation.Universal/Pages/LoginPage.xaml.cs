using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Security.Credentials;
using Windows.Security.Credentials.UI;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Famoser.Bookmarked.Business.Services;
using Famoser.Bookmarked.Presentation.Universal.Models;
using Famoser.Bookmarked.View.ViewModels;
using Famoser.FrameworkEssentials.Logging;
using Famoser.FrameworkEssentials.UniversalWindows.Platform;
using Newtonsoft.Json;

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

        private LoginViewModel LoginViewModel => DataContext as LoginViewModel;

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (LoginViewModel.IsFirstTime)
            {
                if (PasswordBox1.Password == "" || PasswordBox2.Password == "")
                    return;
                if (PasswordBox1.Password != PasswordBox2.Password)
                    return;
            }
            if (e.Key == VirtualKey.Accept || e.Key == VirtualKey.Enter)
            {
                if (LoginViewModel.LoginCommand.CanExecute(null))
                    LoginViewModel.LoginCommand.Execute(null);
            }
        }
    }
}
