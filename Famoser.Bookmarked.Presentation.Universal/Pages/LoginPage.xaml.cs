using System;
using System.ComponentModel;
using Windows.Foundation.Metadata;
using Windows.Security.Credentials;
using Windows.Security.Credentials.UI;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Famoser.Bookmarked.Presentation.Universal.Models;
using Famoser.Bookmarked.View.ViewModels;
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
            InitializeAlternativeLogins();
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

        private void InitializeAlternativeLogins()
        {
            LoginViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName == "IsFirstTime")
                    if (!LoginViewModel.IsFirstTime)
                        TryWindowsHelloLogin();
            };
            //race condition here
            if (!LoginViewModel.IsFirstTime)
                TryWindowsHelloLogin();
        }

        private const string AccountId = "Bookmarked";

        private async void TryWindowsHelloLogin()
        {
            try
            {
                var ss = new StorageService();
                var pwInfoFile = await ss.GetCachedTextFileAsync(".pw_info");
                var pwFile = await ss.GetCachedFileAsync(".pw");
                if (pwInfoFile != null)
                {
                    var model = JsonConvert.DeserializeObject<PasswordInfoModel>(pwInfoFile);
                }
                else
                {
                    var keyCredentialAvailable = await KeyCredentialManager.IsSupportedAsync();
                    if (!keyCredentialAvailable)
                    {
                        // User didn't set up PIN yet
                        return;
                    }

                    var pwInfoModel = new PasswordInfoModel();
                    var keyCreationResult = await KeyCredentialManager.RequestCreateAsync(AccountId, KeyCredentialCreationOption.ReplaceExisting);
                    if (keyCreationResult.Status == KeyCredentialStatus.Success)
                    {
                        var userKey = keyCreationResult.Credential;
                        pwInfoModel.KeyId = userKey.Name;
                        pwInfoModel.AccountId = AccountId;
                    }
                    else if (keyCreationResult.Status == KeyCredentialStatus.UserCanceled ||
                             keyCreationResult.Status == KeyCredentialStatus.UserPrefersPassword)
                    {

                    }

                    UserConsentVerificationResult consentResult = await UserConsentVerifier.RequestVerificationAsync("userMessage");
                    if (consentResult.Equals(UserConsentVerificationResult.Verified))
                    {

                    }

                    var openKeyResult = await KeyCredentialManager.OpenAsync(AccountId);

                    if (openKeyResult.Status == KeyCredentialStatus.Success)
                    {
                        var userKey = openKeyResult.Credential;
                        var publicKey = userKey.RetrievePublicKey();
                        /*
                        var signResult = await userKey.RequestSignAsync();

                        if (signResult.Status == KeyCredentialStatus.Success)
                        {
                            return signResult.Result;
                        }
                        else if (signResult.Status == KeyCredentialStatus.UserPrefersPassword)
                        {

                        }
                        */
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
