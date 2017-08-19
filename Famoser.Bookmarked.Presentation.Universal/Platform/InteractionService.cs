using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Famoser.Bookmarked.View.Services.Interfaces;
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

        public async Task<bool> ConfirmMessageAsync(string message)
        {
            var dialog = new MessageDialog(message);

            dialog.Commands.Add(new UICommand("Confirm") { Id = 0 });
            dialog.Commands.Add(new UICommand("Abort") { Id = 1 });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            return (int)result.Id == 0;
        }

        public async Task ShowMessageAsync(string message)
        {
            var dialog = new MessageDialog(message);

            dialog.Commands.Add(new UICommand("Confirm") { Id = 0 });

            dialog.DefaultCommandIndex = 0;

            await dialog.ShowAsync();
        }

        public void CopyToClipboard(string message)
        {
            var package = new DataPackage();
            package.SetText(message);
            Clipboard.SetContent(package);
        }

        public void CloseApplication()
        {
            Application.Current.Exit();
        }

        public async Task<bool> ClearCacheAsync()
        {
            var files = (await ApplicationData.Current.LocalCacheFolder.GetFilesAsync());
            foreach (var file in files)
            {
                await file.DeleteAsync(StorageDeleteOption.Default);
            }
            return true;
        }

        public async Task<bool> SaveFileAsync(string content, string extension, string defaultFileName)
        {
            try
            {
                extension = "." + extension;
                var fileSavePicker = new FileSavePicker();
                fileSavePicker.FileTypeChoices.Add(defaultFileName.Replace('_', ' ') + " file", new List<string> { extension });
                fileSavePicker.DefaultFileExtension = extension;
                fileSavePicker.SuggestedFileName = defaultFileName + extension;

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

        public async Task<string> GetFileContentAsync(string extension)
        {
            try
            {
                extension = "." + extension;
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
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
    }
}
