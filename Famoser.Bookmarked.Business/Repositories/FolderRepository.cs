using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Extensions;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Repositories.Interfaces;

namespace Famoser.Bookmarked.Business.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly IApiRepository<Folder, CollectionModel> _folderRepository;
        private readonly IApiRepository<Entry, CollectionModel> _entryRepository;

        public FolderRepository(IApiService apiService)
        {
            _folderRepository = apiService.ResolveRepository<Folder>();
            _entryRepository = apiService.ResolveRepository<Entry>();
        }

        private ObservableCollection<Folder> _folders;
        private ObservableCollection<Entry> _entries;

        private readonly Dictionary<Guid, Folder> _folderDic = new Dictionary<Guid, Folder>();
        private readonly Dictionary<Guid, List<Folder>> _missingFolderParents = new Dictionary<Guid, List<Folder>>();
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
            throw new NotImplementedException();
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
                    RemoveFolders(notifyCollectionChangedEventArgs.NewItems as IList<Folder>);
                    AddFolders(notifyCollectionChangedEventArgs.NewItems as IList<Folder>);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _root.Folders.Clear();
                    _garbage.Folders.Clear();
                    _folderDic.Clear();
                    _missingFolderParents.Clear();
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
                        //parent not found; put into waiting list
                        if (_missingFolderParents.ContainsKey(folderParentId))
                        {
                            _missingFolderParents.Remove(folderParentId);
                        }
                    }
                }
            }
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
            folder.IsDeleted = true;
            //todo: delete in structure
            return _folderRepository.SaveAsync(folder);
        }

        public Folder CreateFolderAsync(Folder parentFolder)
        {
            var entry = new Folder { };
            parentFolder.Folders.AddUniqueSorted(entry);
            entry.ParentIds.Add(parentFolder.GetId());
            return entry;
        }

        public Task<bool> SaveEntryAsync(Entry entry)
        {
            return _entryRepository.SaveAsync(entry);
        }

        public Task<bool> RemoveEntryAsync(Entry entry)
        {
            entry.IsDeleted = true;
            //todo: delete in structure
            return _entryRepository.RemoveAsync(entry);
        }

        public Entry CreateEntryAsync(Folder parentFolder)
        {
            var entry = new Entry { };
            parentFolder.Entries.AddUniqueSorted(entry);
            entry.ParentIds.Add(parentFolder.GetId());
            return entry;
        }
    }
}
