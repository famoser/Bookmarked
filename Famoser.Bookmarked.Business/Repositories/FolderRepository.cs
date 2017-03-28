using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Extensions;
using Famoser.Bookmarked.Business.Helper;
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
        private readonly IApiRepository<Folder, CollectionModel> _folderRepository;
        private readonly IApiRepository<Entry, CollectionModel> _entryRepository;

        private readonly IPasswordService _passwordService;
        private readonly IEncryptionService _encryptionService;

        public FolderRepository(IApiService apiService, IPasswordService passwordService, IEncryptionService encryptionService)
        {
            _folderRepository = apiService.ResolveRepository<Folder>();
            _entryRepository = apiService.ResolveRepository<Entry>();
            _passwordService = passwordService;
            _encryptionService = encryptionService;
        }

        private ObservableCollection<Folder> _folders;
        private ObservableCollection<Entry> _entries;

        private readonly Dictionary<Guid, Folder> _folderDic = new Dictionary<Guid, Folder>();
        private readonly Dictionary<Guid, List<Folder>> _missingFolderParents = new Dictionary<Guid, List<Folder>>();
        private readonly Dictionary<Guid, List<Entry>> _missingEntryParents = new Dictionary<Guid, List<Entry>>();
        private readonly Folder _root = new Folder { Name = "root", Description = "the root folder" };
        private readonly Folder _garbage = new Folder { Name = "garbage", Description = "the garbage collection" };

        public Folder GetRootFolder()
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
                    AddEntries(notifyCollectionChangedEventArgs.NewItems as IList<Entry>);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveEntries(notifyCollectionChangedEventArgs.NewItems as IList<Entry>);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveEntries(notifyCollectionChangedEventArgs.OldItems as IList<Entry>);
                    AddEntries(notifyCollectionChangedEventArgs.NewItems as IList<Entry>);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveAllEntries();
                    AddEntries(_entries);
                    break;
            }
        }

        private void AddEntries(IList<Entry> entries)
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
                                _missingEntryParents[entryParentId] = new List<Entry>();
                            }

                            _missingEntryParents[entryParentId].Add(entry);
                        }
                    }
                }
            }
        }

        private void RemoveEntries(IList<Entry> entries)
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
                    AddFolders(notifyCollectionChangedEventArgs.NewItems as IList<Folder>);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveFolders(notifyCollectionChangedEventArgs.NewItems as IList<Folder>);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveFolders(notifyCollectionChangedEventArgs.OldItems as IList<Folder>);
                    AddFolders(notifyCollectionChangedEventArgs.NewItems as IList<Folder>);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveAllFolders(_garbage);
                    RemoveAllFolders(_root);
                    AddFolders(_folders);
                    break;
            }
        }

        private void AddFolders(IList<Folder> list)
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
                                _missingFolderParents[folderParentId] = new List<Folder>();
                            }
                            _missingFolderParents[folderParentId].Add(folder);
                        }
                    }
                }
            }

        }

        private void RemoveFolders(IList<Folder> list)
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

        private void RemoveAllFolders(Folder folder)
        {
            foreach (var item in folder.Folders)
            {
                RemoveAllFolders(item);
            }
            folder.Folders.Clear();
        }

        public async Task<bool> SyncAsnyc()
        {
            var res = await _folderRepository.SyncAsync();
            //enforce both try to sync
            return await _entryRepository.SyncAsync() && res;
        }

        public Task<bool> SaveFolderAsync(Folder folder)
        {
            return _folderRepository.SaveAsync(folder);
        }

        public Task<bool> RemoveFolderAsync(Folder folder)
        {
            RemoveFolders(new List<Folder>() { folder });
            folder.IsDeleted = true;
            AddFolders(new List<Folder>() { folder });
            return _folderRepository.SaveAsync(folder);
        }

        public Folder CreateFolderAsync(Folder parentFolder)
        {
            var entry = new Folder { };
            parentFolder.Folders.AddUniqueSorted(entry);
            entry.ParentIds.Add(parentFolder.GetId());
            return entry;
        }

        public async Task<bool> SaveEntryAsync(Entry entry, ContentModel contentModel)
        {
            try
            {
                var json = JsonConvert.SerializeObject(contentModel);
                var pw = await _passwordService.GetPasswordAsync();
                var encrypted = _encryptionService.GetContentAsync(json, pw);
                entry.Content = encrypted;
                entry.ContentType = contentModel.GetContentType();
                return await _entryRepository.SaveAsync(entry);
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e, this);
                return false;
            }
        }

        public Task<bool> RemoveEntryAsync(Entry entry)
        {
            entry.IsDeleted = true;
            RemoveEntries(new List<Entry>() { entry });
            AddEntries(new List<Entry>() { entry });
            return _entryRepository.RemoveAsync(entry);
        }

        public Entry CreateEntryAsync(Folder parentFolder)
        {
            var entry = new Entry { };
            parentFolder.Entries.AddUniqueSorted(entry);
            entry.ParentIds.Add(parentFolder.GetId());
            return entry;
        }

        public async Task<ContentModel> GetEntryContent(Entry entry)
        {
            try
            {
                var pw = await _passwordService.GetPasswordAsync();
                var json = _encryptionService.GetContentAsync(entry.Content, pw);
                return ContentTypeHelper.Deserialize(json, entry.ContentType);
            }
            catch (Exception e)
            {
                LogHelper.Instance.LogException(e, this);
                return null;
            }
        }
    }
}
