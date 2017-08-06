using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;

namespace Famoser.Bookmarked.Business.Repositories.FolderRepository
{
    public partial class FolderRepository
    {
        private readonly ConcurrentDictionary<string, EntryModel> _entryLookup = new ConcurrentDictionary<string, EntryModel>();
        private readonly ConcurrentDictionary<string, FolderModel> _folderLookup = new ConcurrentDictionary<string, FolderModel>();

        private DateTime _entryLookupChangeDateTime = DateTime.MaxValue;
        private DateTime _folderLookupChangeDateTime = DateTime.MaxValue;

        private string GetSearchString(FolderModel folder)
        {
            return (folder.Name + " " + folder.Description + " ").ToLowerInvariant();
        }

        private string GetSearchString(EntryModel entry)
        {
            return (entry.Name + " " + entry.Description + " " + entry.IconUri?.AbsoluteUri + " ").ToLowerInvariant();
        }

        private void AddToSearchIndex(FolderModel folder)
        {
            var myKey = GetSearchString(folder);
            while (!_folderLookup.TryAdd(myKey, folder))
            {
                //add a character which likely won't be searched for
                myKey += "_";
            }
            _folderLookupChangeDateTime = DateTime.Now;
        }

        private void AddToSearchIndex(EntryModel entry)
        {
            var myKey = GetSearchString(entry);
            while (!_entryLookup.TryAdd(myKey, entry))
            {
                //add a character which likely won't be searched for
                myKey += "_";
            }
            _entryLookupChangeDateTime = DateTime.Now;
        }

        private void RemoveFromSearchIndex(FolderModel folder)
        {
            //remove from caches
            foreach (var result in _folderSearchCache.Values)
            {
                if (result.Contains(folder))
                {
                    result.Remove(folder);
                }
            }

            var key = GetSearchString(folder);

            //try a fast approach
            if (_folderLookup.ContainsKey(key))
            {
                if (_folderLookup[key] == folder)
                {
                    _folderLookup.TryRemove(key, out var _);
                    return;
                }
            }

            //brute force
            foreach (var folderModel in _folderLookup.ToArray())
            {
                if (folderModel.Value == folder)
                {
                    _folderLookup.TryRemove(folderModel.Key, out var _);
                }
            }
        }

        private void RemoveFromSearchIndex(EntryModel entry)
        {
            //remove from caches
            foreach (var result in _entrySearchCache.Values)
            {
                if (result.Contains(entry))
                {
                    result.Remove(entry);
                }
            }

            var key = GetSearchString(entry);

            //try a fast approach
            if (_entryLookup.ContainsKey(key))
            {
                if (_entryLookup[key] == entry)
                {
                    _entryLookup.TryRemove(key, out var _);
                    return;
                }
            }

            //brute force
            foreach (var entryModel in _entryLookup.ToArray())
            {
                if (entryModel.Value == entry)
                {
                    _folderLookup.TryRemove(entryModel.Key, out var _);
                }
            }
        }

        private readonly ConcurrentDictionary<string, ObservableCollection<EntryModel>> _entrySearchCache = new ConcurrentDictionary<string, ObservableCollection<EntryModel>>();
        private readonly ConcurrentDictionary<string, ObservableCollection<FolderModel>> _folderSearchCache = new ConcurrentDictionary<string, ObservableCollection<FolderModel>>();

        private readonly ConcurrentDictionary<string, DateTime> _entrySearchCacheHistory = new ConcurrentDictionary<string, DateTime>();
        private readonly ConcurrentDictionary<string, DateTime> _folderSearchCacheHistory = new ConcurrentDictionary<string, DateTime>();

        private readonly ConcurrentStack<string> _entryNewSearches = new ConcurrentStack<string>();
        private readonly ConcurrentStack<string> _folderNewSearches = new ConcurrentStack<string>();

        public ObservableCollection<EntryModel> SearchEntry(string searchTerm)
        {
            if (searchTerm.Length < 3)
            {
                return new ObservableCollection<EntryModel>();
            }
            if (!_entrySearchCache.ContainsKey(searchTerm))
            {
                _entrySearchCache.TryAdd(searchTerm, new ObservableCollection<EntryModel>());
                _entryNewSearches.Push(searchTerm);
                EnsureSearchEntryTaskActive();
            }
            return _entrySearchCache[searchTerm];
        }

        private Task _entrySearchTask;
        private void EnsureSearchEntryTaskActive()
        {
            if (_entrySearchTask == null || _entrySearchTask.IsCompleted)
            {
                _entrySearchTask = Task.Run(() => SearchEntries());
            }
        }

        private void SearchEntries()
        {
            while (_entryNewSearches.TryPop(out var search))
            {
                if (_entrySearchCacheHistory.ContainsKey(search))
                {
                    if (_entrySearchCacheHistory[search] > _entryLookupChangeDateTime)
                    {
                        return;
                    }
                }

                _entrySearchCache[search].Clear();
                foreach (var entryLookupKey in _entryLookup.Keys)
                {
                    if (
                        //if entry contains search parameter
                        entryLookupKey.Contains(search) ||

                        //if search parameter contains entry
                        search.Contains(entryLookupKey)
                    )
                    {
                        _entrySearchCache[search].Add(_entryLookup[entryLookupKey]);
                    }
                    //can make search arbitrary complex! try with weighting etc
                }

                _entrySearchCacheHistory[search] = DateTime.Now;
            }
        }

        public ObservableCollection<FolderModel> SearchFolder(string searchTerm)
        {
            if (searchTerm.Length < 3)
            {
                return new ObservableCollection<FolderModel>();
            }
            if (!_folderSearchCache.ContainsKey(searchTerm))
            {
                _folderSearchCache.TryAdd(searchTerm, new ObservableCollection<FolderModel>());
                _folderNewSearches.Push(searchTerm);
                EnsureSearchFolderTaskActive();
            }
            return _folderSearchCache[searchTerm];
        }

        private Task _folderSearchTask;
        private void EnsureSearchFolderTaskActive()
        {
            if (_folderSearchTask == null || _folderSearchTask.IsCompleted)
            {
                _folderSearchTask = Task.Run(() => SearchFolders());
            }
        }

        private void SearchFolders()
        {
            while (_folderNewSearches.TryPop(out var search))
            {
                if (_folderSearchCacheHistory.ContainsKey(search))
                {
                    if (_folderSearchCacheHistory[search] > _folderLookupChangeDateTime)
                    {
                        return;
                    }
                }

                _folderSearchCache[search].Clear();
                foreach (var folderLookupKey in _folderLookup.Keys)
                {
                    if (
                        //if entry contains search parameter
                        folderLookupKey.Contains(search) ||

                        //if search parameter contains entry
                        search.Contains(folderLookupKey)
                    )
                    {
                        _folderSearchCache[search].Add(_folderLookup[folderLookupKey]);
                    }
                    //can make search arbitrary complex! try with weighting etc
                }

                _folderSearchCacheHistory[search] = DateTime.Now;
            }
        }
    }
}
