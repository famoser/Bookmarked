using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.FrameworkEssentials.Logging;

namespace Famoser.Bookmarked.Presentation.Universal.Platform
{
    public class ImportExportService : IImportExportService
    {
        private async Task<string> ImportFileAsync(string extension)
        {
            try
            {
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
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
    }
}
