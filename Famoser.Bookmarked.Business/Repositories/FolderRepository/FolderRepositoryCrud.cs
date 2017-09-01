using System;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries.Base;
using Famoser.FrameworkEssentials.Logging;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Repositories.FolderRepository
{
    public partial class FolderRepository
    {

        public FolderModel CreateFolder(FolderModel parentFolderModel)
        {
            var entry = new FolderModel();
            entry.ParentIds.Add(parentFolderModel.GetId());
            return entry;
        }

        public Task<bool> SaveFolderAsync(FolderModel folderModel)
        {
            return _folderRepository.SaveAsync(folderModel);
        }

        public async Task<bool> RemoveFolderAsync(FolderModel folderModel)
        {
            //removing parent ids
            foreach (var entryModel in _entries)
            {
                if (entryModel.ParentIds.Contains(folderModel.GetId()))
                {
                    entryModel.ParentIds.Remove(folderModel.GetId());
                    await _entryRepository.SaveAsync(entryModel);
                }
            }
            foreach (var folder in _folders)
            {
                if (folder.ParentIds.Contains(folderModel.GetId()))
                {
                    folder.ParentIds.Remove(folderModel.GetId());
                    await _folderRepository.SaveAsync(folder);
                }
            }
            return await _folderRepository.RemoveAsync(folderModel);
        }

        public EntryModel CreateEntry(FolderModel parentFolderModel, ContentType type)
        {
            var entry = new EntryModel { ContentType = type };
            entry.ParentIds.Add(parentFolderModel.GetId());
            return entry;
        }

        public async Task<bool> SaveEntryAsync(EntryModel entryModel, ContentModel contentModel)
        {
            try
            {
                var json = JsonConvert.SerializeObject(contentModel);
                var pw = _passwordService.GetPassword();
                if (pw == null)
                    return false;
                var encrypted = _encryptionService.Encrypt(json, pw);
                entryModel.Content = encrypted;
                entryModel.ContentType = contentModel.GetContentType();
                return await _entryRepository.SaveAsync(entryModel);
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e, this);
                return false;
            }
        }

        public Task<bool> RemoveEntryAsync(EntryModel entryModel)
        {
            return _entryRepository.RemoveAsync(entryModel);
        }

        public T GetEntryContent<T>(EntryModel entryModel) where T : ContentModel, new()
        {
            try
            {
                if (!string.IsNullOrEmpty(entryModel.Content))
                {
                    var pw = _passwordService.GetPassword();
                    if (pw != null)
                    {
                        var json = _encryptionService.Decrypt(entryModel.Content, pw);
                        return JsonConvert.DeserializeObject<T>(json);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e, this);
            }
            var newObj = new T();
            newObj.SetDefaultValues();
            return newObj;
        }
    }
}
