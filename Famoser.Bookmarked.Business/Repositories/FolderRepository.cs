using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
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
                    AddFoldersFromNotify(notifyCollectionChangedEventArgs);
                    break;
            }
        }

        private void AddFoldersFromNotify(NotifyCollectionChangedEventArgs args)
        {
            foreach (var newItem in args.NewItems)
            {
                if (newItem is Folder folder)
                {
                    if (!folder.IsDeleted)
                    {
                        //put it into lookup
                        _folderDic[folder.GetId()] = folder;
                        //add elements in waiting queue
                        if (_missingFolderParents.ContainsKey(folder.GetId()))
                        {
                            foreach (var folder1 in _missingFolderParents[folder.GetId()])
                            {
                                folder.Folders.Add(folder1);
                            }
                            _missingFolderParents.Remove(folder.GetId());
                        }

                        //look for parent
                        if (folder.ParentId == Guid.Empty)
                        {
                            _root.Folders.Add(folder);
                        }
                        else
                        {
                            if (_folderDic.ContainsKey(folder.ParentId))
                            {
                                _folderDic[folder.ParentId].Folders.Add(folder);
                            }
                            else
                            {
                                //parent not found; put into waiting list
                                if (!_missingFolderParents.ContainsKey(folder.ParentId))
                                {
                                    _missingFolderParents[folder.ParentId] = new List<Folder>();
                                }
                                _missingFolderParents[folder.ParentId].Add(folder);
                            }
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
            folder.Parent.Folders.Remove(folder);
            return _folderRepository.RemoveAsync(folder);
        }

        public Folder CreateFolderAsync(Folder parentFolder)
        {
            var entry = new Folder { Parent = parentFolder };
            return entry;
        }

        public Task<bool> SaveEntryAsync(Entry entry)
        {
            return _entryRepository.SaveAsync(entry);
        }

        public Task<bool> RemoveEntryAsync(Entry entry)
        {
            entry.IsDeleted = true;
            entry.Parent.Folders.Remove(entry);
            return _entryRepository.RemoveAsync(entry);
        }

        public Entry CreateEntryAsync(Folder parentFolder)
        {
            var entry = new Entry { Parent = parentFolder };
            return entry;
        }
    }
}
