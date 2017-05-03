using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Famoser.Bookmarked.View.Services.Interfaces;
using GalaSoft.MvvmLight.Threading;

namespace Famoser.Bookmarked.Presentation.Universal.Platform
{
    internal class InteractionService : IInteractionService
    {
        public void OpenInBrowser(Uri uri)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Windows.System.Launcher.LaunchUriAsync(uri);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    
        public void CheckBeginInvokeOnUi(Action action)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(action);
        }

        public async Task<bool> ConfirmMessage(string message)
        {
            var dialog = new MessageDialog( message);

            dialog.Commands.Add(new UICommand("Confirm") { Id = 0 });
            dialog.Commands.Add(new UICommand("Abort") { Id = 1 });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            return (int)result.Id == 0;
        }

        public void CopyToClipboard(string message)
        {
            var package = new DataPackage();
            package.SetText(message);
            Clipboard.SetContent(package);
        }

        public string HashPassword(string password)
        {
            // put the string in a buffer
            IBuffer input = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);

            // hash it with SHA 256
            var hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            IBuffer hashed = hasher.HashData(input);

            // return as base64
            return CryptographicBuffer.EncodeToBase64String(hashed);
        }
    }
}
