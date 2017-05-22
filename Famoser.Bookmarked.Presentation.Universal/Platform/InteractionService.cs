using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels;
using Famoser.FrameworkEssentials.Logging;
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
            var dialog = new MessageDialog(message);

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

        private async Task<string> ImportFileAsync(string extension)
        {
            try
            {
                var picker =
                    new Windows.Storage.Pickers.FileOpenPicker
                    {
                        ViewMode = Windows.Storage.Pickers.PickerViewMode.List,
                        SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
                    };

                picker.FileTypeFilter.Add(extension);

                StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    return await FileIO.ReadTextAsync(file);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return null;
        }

        public Task<string> ImportExportFileAsync()
        {
            return ImportFileAsync(".bmd_data");
        }

        public Task<string> ImportCredentialsFileAsync()
        {
            return ImportFileAsync(".bmd_cred");
        }

        private async Task<bool> ExportFileAsync(string content, string extension, string fileName)
        {
            try
            {
                var fileSavePicker = new FileSavePicker();
                fileSavePicker.FileTypeChoices.Add(fileName.Replace('_', ' ') + " file", new List<string> { extension });
                fileSavePicker.DefaultFileExtension = extension;
                fileSavePicker.SuggestedFileName = fileName + extension;

                StorageFile file = await fileSavePicker.PickSaveFileAsync();
                if (file != null && content != null)
                {
                    await FileIO.WriteTextAsync(file, content);
                }
                return true;
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e);
            }
            return false;
        }

        public Task<bool> SaveExportFileAsync(string content)
        {
            return ExportFileAsync(content, ".bmd_data", "bookmarked");
        }

        public Task<bool> SaveCredentialsFileAsync(string content)
        {
            return ExportFileAsync(content, ".bmd_cred", "bookmarked_credentials");
        }

        public void CloseApplication()
        {
            Application.Current.Exit();
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
