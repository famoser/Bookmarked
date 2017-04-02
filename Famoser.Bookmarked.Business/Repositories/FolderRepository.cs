using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Extensions;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries.Base;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.FrameworkEssentials.Logging;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Repositories.Interfaces;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly IApiRepository<FolderModel, CollectionModel> _folderRepository;
        private readonly IApiRepository<EntryModel, CollectionModel> _entryRepository;

        private readonly IPasswordService _passwordService;
        private readonly IEncryptionService _encryptionService;

        public FolderRepository(IApiService apiService, IPasswordService passwordService, IEncryptionService encryptionService)
        {
            _folderRepository = apiService.ResolveRepository<FolderModel>();
            _entryRepository = apiService.ResolveRepository<EntryModel>();
            _passwordService = passwordService;
            _encryptionService = encryptionService;
        }

        private ObservableCollection<FolderModel> _folders;
        private ObservableCollection<EntryModel> _entries;

        private readonly Dictionary<Guid, FolderModel> _folderDic = new Dictionary<Guid, FolderModel>();
        private readonly Dictionary<Guid, List<FolderModel>> _missingFolderParents = new Dictionary<Guid, List<FolderModel>>();
        private readonly Dictionary<Guid, List<EntryModel>> _missingEntryParents = new Dictionary<Guid, List<EntryModel>>();
        private readonly FolderModel _root = new FolderModel { Name = "root", Description = "the root folder" };
        private readonly FolderModel _garbage = new FolderModel { Name = "garbage", Description = "the garbage collection" };

        public FolderModel GetRootFolder()
        {
            lock (this)
            {
                if (_folders == null)
                {
                    _folders = _folderRepository.GetAllLazy();
                    _folders.CollectionChanged += FoldersOnCollectionChanged;
                    _entries = _entryRepository.GetAllLazy();
                    _entries.CollectionChanged += EntriesOnCollectionChanged;
                }
            }
            return _root;
        }

        private void EntriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddEntries(notifyCollectionChangedEventArgs.NewItems as IList<EntryModel>);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveEntries(notifyCollectionChangedEventArgs.NewItems as IList<EntryModel>);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveEntries(notifyCollectionChangedEventArgs.OldItems as IList<EntryModel>);
                    AddEntries(notifyCollectionChangedEventArgs.NewItems as IList<EntryModel>);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveAllEntries();
                    AddEntries(_entries);
                    break;
            }
        }

        private void AddEntries(IList<EntryModel> entries)
        {
            foreach (var entry in entries)
            {
                if (entry.IsDeleted)
                {
                    _garbage.Entries.Add(entry);
                }
                else
                {
                    foreach (var entryParentId in entry.ParentIds)
                    {
                        if (_folderDic.ContainsKey(entryParentId))
                        {
                            _folderDic[entryParentId].Entries.Add(entry);
                        }
                        else
                        {
                            if (!_missingEntryParents.ContainsKey(entryParentId))
                            {
                                _missingEntryParents[entryParentId] = new List<EntryModel>();
                            }

                            _missingEntryParents[entryParentId].Add(entry);
                        }
                    }
                }
            }
        }

        private void RemoveEntries(IList<EntryModel> entries)
        {
            foreach (var entry in entries)
            {
                if (_garbage.Entries.Contains(entry))
                {
                    _garbage.Entries.Remove(entry);
                }
                else
                {
                    foreach (var entryParentId in entry.ParentIds)
                    {
                        if (_folderDic.ContainsKey(entryParentId))
                        {
                            _folderDic[entryParentId].Entries.Remove(entry);
                        }
                        else
                        {
                            _missingEntryParents[entryParentId].Remove(entry);
                        }
                    }
                }
            }
        }

        private void RemoveAllEntries()
        {
            foreach (var folder in _folders)
            {
                folder.Entries.Clear();
            }
        }

        private void FoldersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddFolders(notifyCollectionChangedEventArgs.NewItems as IList<FolderModel>);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveFolders(notifyCollectionChangedEventArgs.NewItems as IList<FolderModel>);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveFolders(notifyCollectionChangedEventArgs.OldItems as IList<FolderModel>);
                    AddFolders(notifyCollectionChangedEventArgs.NewItems as IList<FolderModel>);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveAllFolders(_garbage);
                    RemoveAllFolders(_root);
                    AddFolders(_folders);
                    break;
            }
        }

        private void AddFolders(IList<FolderModel> list)
        {
            foreach (var folder in list)
            {
                //put it into lookup
                _folderDic[folder.GetId()] = folder;
                //add elements in waiting queue
                if (_missingFolderParents.ContainsKey(folder.GetId()))
                {
                    foreach (var folder1 in _missingFolderParents[folder.GetId()])
                    {
                        folder.Folders.AddUniqueSorted(folder1);
                    }
                    _missingFolderParents.Remove(folder.GetId());
                }
                if (_missingEntryParents.ContainsKey(folder.GetId()))
                {
                    foreach (var folder1 in _missingEntryParents[folder.GetId()])
                    {
                        folder.Entries.AddUniqueSorted(folder1);
                    }
                    _missingEntryParents.Remove(folder.GetId());
                }

                foreach (var folderParentId in folder.ParentIds)
                {
                    //look for parent
                    if (folderParentId == Guid.Empty)
                    {
                        if (folder.IsDeleted)
                            _garbage.Folders.AddUniqueSorted(folder);
                        else
                            _root.Folders.AddUniqueSorted(folder);
                    }
                    else
                    {
                        if (_folderDic.ContainsKey(folderParentId))
                        {
                            if (folder.IsDeleted)
                            {
                                if (_folderDic[folderParentId].IsDeleted)
                                    _folderDic[folderParentId].Folders.AddUniqueSorted(folder);
                                else
                                    _garbage.Folders.AddUniqueSorted(folder);
                            }
                            else
                                _folderDic[folderParentId].Folders.AddUniqueSorted(folder);
                        }
                        else
                        {
                            //parent not found; put into waiting list
                            if (!_missingFolderParents.ContainsKey(folderParentId))
                            {
                                _missingFolderParents[folderParentId] = new List<FolderModel>();
                            }
                            _missingFolderParents[folderParentId].Add(folder);
                        }
                    }
                }
            }

        }

        private void RemoveFolders(IList<FolderModel> list)
        {
            foreach (var folder in list)
            {
                foreach (var folderParentId in folder.ParentIds)
                {
                    if (_folderDic.ContainsKey(folderParentId))
                    {
                        if (folder.IsDeleted)
                        {
                            if (_folderDic[folderParentId].IsDeleted)
                                _folderDic[folderParentId].Folders.Remove(folder);
                            else
                                _garbage.Folders.Remove(folder);
                        }
                        else
                            _folderDic[folderParentId].Folders.Remove(folder);
                    }
                    else
                    {
                        //parent not found
                        if (_missingFolderParents.ContainsKey(folderParentId))
                        {
                            _missingFolderParents.Remove(folderParentId);
                        }
                        //parent not found
                        if (_missingEntryParents.ContainsKey(folderParentId))
                        {
                            _missingEntryParents.Remove(folderParentId);
                        }
                    }
                }
            }
        }

        private void RemoveAllFolders(FolderModel folderModel)
        {
            foreach (var item in folderModel.Folders)
            {
                RemoveAllFolders(item);
            }
            folderModel.Folders.Clear();
        }

        public async Task<bool> SyncAsnyc()
        {
            var res = await _folderRepository.SyncAsync();
            //enforce both try to sync
            return await _entryRepository.SyncAsync() && res;
        }

        public Task<bool> SaveFolderAsync(FolderModel folderModel)
        {
            return _folderRepository.SaveAsync(folderModel);
        }

        public Task<bool> RemoveFolderAsync(FolderModel folderModel)
        {
            RemoveFolders(new List<FolderModel>() { folderModel });
            folderModel.IsDeleted = true;
            AddFolders(new List<FolderModel>() { folderModel });
            return _folderRepository.SaveAsync(folderModel);
        }

        public FolderModel CreateFolder(FolderModel parentFolderModel)
        {
            var entry = new FolderModel {};
            parentFolderModel.Folders.AddUniqueSorted(entry);
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
            entryModel.IsDeleted = true;
            RemoveEntries(new List<EntryModel>() { entryModel });
            AddEntries(new List<EntryModel>() { entryModel });
            return _entryRepository.RemoveAsync(entryModel);
        }

        public EntryModel CreateEntry(FolderModel parentFolderModel, ContentType type)
        {
            var entry = new EntryModel { ContentType = type};
            parentFolderModel.Entries.AddUniqueSorted(entry);
            entry.ParentIds.Add(parentFolderModel.GetId());
            return entry;
        }

        public T GetEntryContent<T>(EntryModel entryModel) where T : ContentModel, new()
        {
            try
            {
                var pw = _passwordService.GetPassword();
                if (pw == null)
                    return null;
                if (!string.IsNullOrEmpty(entryModel.Content))
                {
                    var json = _encryptionService.Decrypt(entryModel.Content, pw);
                    return JsonConvert.DeserializeObject<T>(json);
                }
                return new T();
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e, this);
                return null;
            }
        }
    }
}
