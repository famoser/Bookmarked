using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Extensions;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Base;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Repositories.Interfaces;

namespace Famoser.Bookmarked.Business.Repositories.FolderRepository
{
    public partial class FolderRepository : IFolderRepository
    {
        private readonly IApiService _apiService;
        private readonly IDispatchService _dispatchService;

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

        //fixed folders
        private readonly Guid _rootGuid = Guid.Parse("2c9cb460-0be3-4612-a411-810371268c9a");
        private readonly Guid _garbageGuid = Guid.Parse("8979801a-c64c-43ad-928c-36f4ff3b6bc0");
        private readonly Guid _parentNotFound = Guid.Parse("b8b6e207-1d54-4498-82fe-81b53faab710");

        public FolderRepository(IApiService apiService, IPasswordService passwordService, IEncryptionService encryptionService, IDispatchService dispatchService)
        {
            _folderRepository = apiService.ResolveRepository<FolderModel>();
            _entryRepository = apiService.ResolveRepository<EntryModel>();
            _passwordService = passwordService;
            _encryptionService = encryptionService;
            _dispatchService = dispatchService;
            _apiService = apiService;

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
                    _entries = _entryRepository.GetAllLazy();
                    AddEntries(_entries.ToList());
                    _entries.CollectionChanged += (s, e) => _dispatchService.CheckBeginInvokeOnUi(() => EntriesOnCollectionChanged(e));
                    AddFolders(_folders.ToList());
                    _folders.CollectionChanged += (s, e) => _dispatchService.CheckBeginInvokeOnUi(() => FoldersOnCollectionChanged(e));
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
                    RemoveEntries(notifyCollectionChangedEventArgs.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveEntries(notifyCollectionChangedEventArgs.OldItems);
                    AddEntries(notifyCollectionChangedEventArgs.NewItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveAllEntriesFast();
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
                _folderDic[_garbageGuid].Entries.AddUniqueSorted(entry);
            }
            else
            {
                var once = false;
                foreach (var entryParentId in entry.ParentIds.Distinct())
                {
                    if (_folderDic.ContainsKey(entryParentId))
                    {
                        once |= _folderDic[entryParentId].Entries.AddUniqueSorted(entry);
                    }
                }
                if (once)
                {
                    AddToSearchIndex(entry);
                }
            }
        }

        private bool FixParentModel(ParentModel entry)
        {
            if (entry == null)
                return false;

            if (entry.ParentIds.Contains(Guid.Empty))
                entry.ParentIds.Remove(Guid.Empty);

            if (entry.ParentIds.Contains(entry.GetId()))
                entry.ParentIds.Remove(entry.GetId());

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
            RemoveFromSearchIndex(entry);
        }

        private void RemoveAllEntriesFast()
        {
            foreach (var folder in _folders)
            {
                foreach (var folderEntry in folder.Entries)
                {
                    RemoveFromSearchIndex(folderEntry);
                }
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
                    RemoveFolders(notifyCollectionChangedEventArgs.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveFolders(notifyCollectionChangedEventArgs.OldItems);
                    AddFolders(notifyCollectionChangedEventArgs.NewItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveAllFoldersFast();
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
            
            //add itself to parents
            if (folder.ParentIds.Contains(_garbageGuid))
            {
                _folderDic[_garbageGuid].Folders.AddUniqueSorted(folder);
            }
            else
            {
                //put it into lookup
                if (!_folderDic.TryAdd(folder.GetId(), folder))
                    return;

                var once = false;
                foreach (var entryParentId in folder.ParentIds.Distinct())
                {
                    if (_folderDic.ContainsKey(entryParentId) && !_folderDic[entryParentId].Folders.Contains(folder))
                    {
                        once |= _folderDic[entryParentId].Folders.AddUniqueSorted(folder);
                    }
                }
                if (once)
                {
                    AddToSearchIndex(folder);
                }
            }

            //look for missing children
            foreach (var entryModel in _entries.ToList())
            {
                if (entryModel.ParentIds.Contains(folder.GetId()))
                {
                    folder.Entries.AddUniqueSorted(entryModel);
                }
            }

            //look for missing folders
            foreach (var folderModel in _folders.ToList())
            {
                if (folderModel.ParentIds.Contains(folder.GetId()) && !folder.Folders.Contains(folderModel))
                {
                    folder.Folders.AddUniqueSorted(folderModel);
                }
            }

            AddToSearchIndex(folder);
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
            foreach (var modelParentId in model.ParentIds.Distinct())
            {
                if (_folderDic.ContainsKey(modelParentId) && _folderDic[modelParentId].Folders.Contains(model))
                    _folderDic[modelParentId].Folders.Remove(model);
            }
            RemoveFromSearchIndex(model);
        }

        private void RemoveAllFoldersFast()
        {
            //remove sub folders
            foreach (var item in _folderDic)
            {
                item.Value.Folders.Clear();
            }

            //remove from search
            foreach (var folderModel in _folderDic.Values)
            {
                RemoveFromSearchIndex(folderModel);
            }

            //remove key
            foreach (var key in _folderDic.Keys.Where(k => k != _garbageGuid && k != _parentNotFound && k != _rootGuid).ToList())
            {
                _folderDic.TryRemove(key, out var _);
            }
        }
        public async Task<bool> SyncAsync()
        {
            var res = await _folderRepository.SyncAsync();
            //enforce both try to sync
            return await _entryRepository.SyncAsync() && res;
        }
    }
}
