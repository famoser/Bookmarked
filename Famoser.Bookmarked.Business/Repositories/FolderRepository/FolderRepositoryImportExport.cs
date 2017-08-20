using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            try
            {
                //ensure everything up to date
                await _folderRepository.SyncAsync();
                await _entryRepository.SyncAsync();

                //create import model
                var importModel = new ImportModel()
                {
                    ExportDate = DateTime.Now,
                    Entries = _entries.ToList(),
                    Folders = _folders.ToList(),
                    CreditCardModels = new Dictionary<Guid, CreditCardModel>(),
                    NoteModels = new Dictionary<Guid, NoteModel>(),
                    OnlineAccountModels = new Dictionary<Guid, OnlineAccountModel>(),
                    WebpageModels = new Dictionary<Guid, WebpageModel>()
                };
                foreach (var importModelEntry in importModel.Entries)
                {
                    switch (importModelEntry.ContentType)
                    {
                        case ContentType.CreditCard:
                            importModel.CreditCardModels.Add(importModelEntry.GetId(), GetEntryContent<CreditCardModel>(importModelEntry));
                            break;
                        case ContentType.Note:
                            importModel.NoteModels.Add(importModelEntry.GetId(), GetEntryContent<NoteModel>(importModelEntry));
                            break;
                        case ContentType.OnlineAccount:
                            importModel.OnlineAccountModels.Add(importModelEntry.GetId(), GetEntryContent<OnlineAccountModel>(importModelEntry));
                            break;
                        case ContentType.Webpage:
                            importModel.WebpageModels.Add(importModelEntry.GetId(), GetEntryContent<WebpageModel>(importModelEntry));
                            break;
                    }
                }
                //encrypt
                var str = JsonConvert.SerializeObject(importModel);
                return _encryptionService.Encrypt(str, _passwordService.GetPassword());
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e);
            }
            return null;
        }

        public async Task<bool> ImportDataAsync(string content, string password)
        {
            try
            {
                //decrypt 
                var decrypted = _encryptionService.Decrypt(content, password);
                var importModel = JsonConvert.DeserializeObject<ImportModel>(decrypted);

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
