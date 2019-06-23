using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.FrameworkEssentials.Logging;
using Famoser.SyncApi.Models;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Repositories.FolderRepository
{
    public partial class FolderRepository
    {

        public async Task<string> ExportDataAsync()
        {
            //todo: switch to .csv export
            try
            {
                //ensure everything up to date
                await _folderRepository.SyncAsync();
                await _entryRepository.SyncAsync();

                using (var memoryStream = new MemoryStream())
                {
                    using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                    {
                        using (var csv = new CsvWriter(streamWriter))
                        {
                            WriteFolderContent(csv, GetRootFolder());

                            streamWriter.Flush();
                            memoryStream.Position = 0;

                            using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e);
            }
            return null;
        }

        private void WriteFolderContent(CsvWriter csv, FolderModel folderModel, string prefix = "")
        {
            var folderName = prefix + "/" + folderModel.Name;
            var exportModels = folderModel.Entries.Select(entry => ConvertToCsvExportEntry(entry, folderName));
            csv.WriteRecords(exportModels);

            foreach (var childFolder in folderModel.Folders)
            {
                WriteFolderContent(csv, childFolder, folderName);
            }
        }

        private CsvExportEntry ConvertToCsvExportEntry(EntryModel entryModel, string folder)
        {
            CsvExportEntry csvExportEntry;
            switch (entryModel.ContentType)
            {
                case ContentType.CreditCard:
                    csvExportEntry = GetEntryContent<CreditCardModel>(entryModel).ConvertToCsvExportEntry();
                    csvExportEntry.ContentType = "Credit Card";
                    break;
                case ContentType.Note:
                    csvExportEntry = GetEntryContent<NoteModel>(entryModel).ConvertToCsvExportEntry();
                    csvExportEntry.ContentType = "Note";
                    break;
                case ContentType.OnlineAccount:
                    csvExportEntry = GetEntryContent<OnlineAccountModel>(entryModel).ConvertToCsvExportEntry();
                    csvExportEntry.ContentType = "Online Account";
                    break;
                case ContentType.Webpage:
                    csvExportEntry = GetEntryContent<WebpageModel>(entryModel).ConvertToCsvExportEntry();
                    csvExportEntry.ContentType = "Webpage";
                    break;
                default:
                    // fail silently to allow export of others
                    LogHelper.Instance.LogError("Tried to export content type " + entryModel.ContentType);
                    csvExportEntry = new CsvExportEntry();
                    break;
            }

            csvExportEntry.Folder = folder;

            return csvExportEntry;
        }

        public async Task<string> GetImportDataTemplateAsync()
        {
            //todo
            return "";
        }

        public async Task<bool> ImportDataAsync(string content)
        {
            try
            {
                //decrypt 
                //TODO: replace with excel import/export
                var importModel = JsonConvert.DeserializeObject<ImportModel>(content);

                foreach (var importModelFolder in importModel.Folders)
                {
                    //pretty sure you have to await to avoid data races
                    await _folderRepository.SaveAsync(importModelFolder);
                }

                var entryDic = new Dictionary<Guid, EntryModel>();
                foreach (var importModelEntry in importModel.Entries)
                {
                    entryDic.Add(importModelEntry.GetId(), importModelEntry);

                    //pretty sure you have to await to avoid data races
                    await _entryRepository.SaveAsync(importModelEntry);
                }

                foreach (var model in importModel.CreditCardModels)
                {
                    await SaveEntryAsync(entryDic[model.Key], model.Value);
                }

                foreach (var model in importModel.NoteModels)
                {
                    await SaveEntryAsync(entryDic[model.Key], model.Value);
                }

                foreach (var model in importModel.OnlineAccountModels)
                {
                    await SaveEntryAsync(entryDic[model.Key], model.Value);
                }

                foreach (var model in importModel.WebpageModels)
                {
                    await SaveEntryAsync(entryDic[model.Key], model.Value);
                }
                return true;
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e);
            }
            return false;
        }

        public async Task<string> ExportCredentialsAsync()
        {
            try
            {
                var user = await _apiService.GetApiUserAsync();
                if (user == null)
                {
                    var roaming = await _apiService.GetApiStorageService().GetApiRoamingEntityAsync();
                    user = new UserModel()
                    {
                        Id = roaming.UserId
                    };
                }
                var json = JsonConvert.SerializeObject(user);
                return _encryptionService.Encrypt(json, _passwordService.GetPassword());
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e);
            }
            return null;
        }

        public async Task<bool> ImportCredentialsAsync(string content, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(content))
                    return false;

                var decrypted = _encryptionService.Decrypt(content, password);
                var newCred = JsonConvert.DeserializeObject<UserModel>(decrypted);
                return await _apiService.SetApiUserAsync(newCred);
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e);
            }
            return false;
        }

        public async Task<bool> ClearAllDataAsync()
        {
            try
            {
                //may fail if someone tries to access file in that particular moment
                var retries = 5;
                while (!await _apiService.GetApiStorageService().EraseRoamingAndCacheAsync() && retries-- > 0)
                {
                    await Task.Delay(300);
                }
                return true;
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e);
            }
            return false;
        }
    }
}
