using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Base;
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
        //repositories
        private readonly IApiRepository<FolderModel, CollectionModel> _folderRepository;
        private readonly IApiRepository<EntryModel, CollectionModel> _entryRepository;

        //repository output
        private ObservableCollection<FolderModel> _folders;
        private ObservableCollection<EntryModel> _entries;

        //lookup
        private readonly ConcurrentDictionary<Guid, FolderModel> _folderDic;

        //services
        private readonly IPasswordService _passwordService;
        private readonly IEncryptionService _encryptionService;
        private readonly IViewService _viewService;

        //fixed folders
        private readonly Guid _rootGuid = Guid.Parse("2c9cb460-0be3-4612-a411-810371268c9a");
        private readonly Guid _garbageGuid = Guid.Parse("8979801a-c64c-43ad-928c-36f4ff3b6bc0");
        private readonly Guid _parentNotFound = Guid.Parse("b8b6e207-1d54-4498-82fe-81b53faab710");

        public FolderRepository(IApiService apiService, IPasswordService passwordService, IEncryptionService encryptionService, IViewService viewService)
        {
            _folderRepository = apiService.ResolveRepository<FolderModel>();
            _entryRepository = apiService.ResolveRepository<EntryModel>();
            _passwordService = passwordService;
            _encryptionService = encryptionService;
            _viewService = viewService;

            _folderDic = new ConcurrentDictionary<Guid, FolderModel>();
            _folderDic.TryAdd(_rootGuid, new FolderModel { Name = "root", Description = "the root folder" });
            _folderDic.TryAdd(_garbageGuid, new FolderModel { Name = "garbage", Description = "the garbage collection" });
            _folderDic.TryAdd(_parentNotFound, new FolderModel());
            _folderDic[_rootGuid].SetId(_rootGuid);
            _folderDic[_garbageGuid].SetId(_garbageGuid);
            _folderDic[_parentNotFound].SetId(_parentNotFound);
        }

        public FolderModel GetRootFolder()
        {
            EnsureInitialized();
            return _folderDic[_rootGuid];
        }

        public FolderModel GetGarbageFolder()
        {
            EnsureInitialized();
            return _folderDic[_garbageGuid];
        }

        private void EnsureInitialized()
        {
            lock (this)
            {
                if (_folders == null)
                {
                    _folders = _folderRepository.GetAllLazy();
                    _folders.CollectionChanged += (s, e) => _viewService.CheckBeginInvokeOnUi(() => FoldersOnCollectionChanged(e));
                    _entries = _entryRepository.GetAllLazy();
                    _entries.CollectionChanged += (s,e) => _viewService.CheckBeginInvokeOnUi(() => EntriesOnCollectionChanged(e));
                }
            }
        }

        private void EntriesOnCollectionChanged(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddEntries(notifyCollectionChangedEventArgs.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveEntries(notifyCollectionChangedEventArgs.NewItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveEntries(notifyCollectionChangedEventArgs.OldItems);
                    AddEntries(notifyCollectionChangedEventArgs.NewItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveAllEntries();
                    AddEntries(_entries);
                    break;
            }
        }

        private void AddEntries(IList entries)
        {
            foreach (var entry in entries)
            {
                AddEntry(entry as EntryModel);
            }
        }

        private void AddEntry(EntryModel entry)
        {
            if (!FixParentModel(entry))
                return;

            if (entry.ParentIds.Contains(_garbageGuid))
            {
                _folderDic[_garbageGuid].Entries.Add(entry);
            }
            else
            {
                foreach (var entryParentId in entry.ParentIds)
                {
                    if (_folderDic.ContainsKey(entryParentId))
                    {
                        _folderDic[entryParentId].Entries.Add(entry);
                    }
                }
            }
        }

        private bool FixParentModel(ParentModel entry)
        {
            if (entry == null)
                return false;

            if (entry.ParentIds.Contains(Guid.Empty))
                entry.ParentIds.Remove(Guid.Empty);

            if (entry.ParentIds.Count == 0)
                entry.ParentIds.Add(_rootGuid);

            return true;
        }

        private void RemoveEntries(IList entries)
        {
            foreach (var entry in entries)
            {
                RemoveEntry(entry as EntryModel);
            }
        }

        private void RemoveEntry(EntryModel entry)
        {
            if (entry == null)
                return;

            foreach (var entryParentId in entry.ParentIds)
            {
                if (_folderDic.ContainsKey(entryParentId) && _folderDic[entryParentId].Entries.Contains(entry))
                {
                    _folderDic[entryParentId].Entries.Remove(entry);
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

        private void FoldersOnCollectionChanged(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddFolders(notifyCollectionChangedEventArgs.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveFolders(notifyCollectionChangedEventArgs.NewItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveFolders(notifyCollectionChangedEventArgs.OldItems);
                    AddFolders(notifyCollectionChangedEventArgs.NewItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveAllFolders();
                    AddFolders(_folders);
                    break;
            }
        }

        private void AddFolders(IList list)
        {
            foreach (var folder in list)
            {
                AddFolder(folder as FolderModel);
            }
        }

        private void AddFolder(FolderModel folder)
        {
            if (!FixParentModel(folder))
                return;

            //put it into lookup
            _folderDic.AddOrUpdate(folder.GetId(), folder, (guid, model) => model);

            //add itself to parents
            if (folder.ParentIds.Contains(_garbageGuid))
            {
                _folderDic[_garbageGuid].Folders.Add(folder);
            }
            else
            {
                foreach (var entryParentId in folder.ParentIds)
                {
                    if (_folderDic.ContainsKey(entryParentId))
                    {
                        _folderDic[entryParentId].Folders.Add(folder);
                    }
                }
            }

            //look for missing children
            foreach (var entryModel in _entries)
            {
                if (entryModel.ParentIds.Contains(folder.GetId()))
                {
                    folder.Entries.Add(entryModel);
                }
            }

            //look for missing folders
            foreach (var folderModel in _folders)
            {
                if (folderModel.ParentIds.Contains(folder.GetId()))
                {
                    folder.Folders.Add(folderModel);
                }
            }
        }

        private void RemoveFolders(IList list)
        {
            foreach (var folder in list)
            {
                RemoveFolder(folder as FolderModel);
            }
        }

        private void RemoveFolder(FolderModel model)
        {
            if (model == null)
                return;

            model.Entries.Clear();
            model.Folders.Clear();
            foreach (var modelParentId in model.ParentIds)
            {
                if (_folderDic.ContainsKey(modelParentId) && _folderDic[modelParentId].Folders.Contains(model))
                    _folderDic[modelParentId].Folders.Remove(model);
            }
        }

        private void RemoveAllFolders()
        {
            //remvoe subfolders
            foreach (var item in _folderDic)
            {
                item.Value.Folders.Clear();
            }

            //remove key
            foreach (var key in _folderDic.Keys.Where(k => k != _garbageGuid && k != _parentNotFound && k != _rootGuid).ToList())
            {
                _folderDic.TryRemove(key, out var folder);
            }
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

        public Task<bool> MoveFolderToGarbageAsync(FolderModel folderModel)
        {
            if (!folderModel.ParentIds.Contains(_garbageGuid))
                folderModel.ParentIds.Add(_garbageGuid);
            RemoveFolder(folderModel);
            AddFolder(folderModel);
            return _folderRepository.SaveAsync(folderModel);
        }

        public Task<bool> MoveFolderOutOfGarbageAsync(FolderModel folderModel)
        {
            if (folderModel.ParentIds.Contains(_garbageGuid))
                folderModel.ParentIds.Remove(_garbageGuid);
            RemoveFolder(folderModel);
            AddFolder(folderModel);
            return _folderRepository.SaveAsync(folderModel);
        }

        public FolderModel CreateFolder(FolderModel parentFolderModel)
        {
            var entry = new FolderModel();
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

        public Task<bool> MoveEntryToGarbageAsync(EntryModel entryModel)
        {
            if (!entryModel.ParentIds.Contains(_garbageGuid))
                entryModel.ParentIds.Add(_garbageGuid);
            RemoveEntry(entryModel);
            AddEntry(entryModel);
            return _entryRepository.SaveAsync(entryModel);
        }

        public Task<bool> MoveEntryOutOfGarbageAsync(EntryModel entryModel)
        {
            if (entryModel.ParentIds.Contains(_garbageGuid))
                entryModel.ParentIds.Remove(_garbageGuid);
            RemoveEntry(entryModel);
            AddEntry(entryModel);
            return _entryRepository.SaveAsync(entryModel);
        }

        public EntryModel CreateEntry(FolderModel parentFolderModel, ContentType type)
        {
            var entry = new EntryModel { ContentType = type };
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

        public Task<bool> RemoveEntryAsync(EntryModel entryModel)
        {
            return _entryRepository.RemoveAsync(entryModel);
        }

        public ObservableCollection<EntryModel> SearchEntry(string searchTerm)
        {
            if (searchTerm.Length < 3)
            {
                return new ObservableCollection<EntryModel>();
            }
            return new ObservableCollection<EntryModel>(_entries.Where(e => e.Name.Contains(searchTerm)));
        }
    }
}
