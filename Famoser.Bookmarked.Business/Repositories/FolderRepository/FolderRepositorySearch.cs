using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.List;
using Famoser.Bookmarked.Business.List.Comparators;
using Famoser.Bookmarked.Business.Models;

namespace Famoser.Bookmarked.Business.Repositories.FolderRepository
{
    public partial class FolderRepository
    {
        private readonly ConcurrentDictionary<string, EntryModel> _entryLookup = new ConcurrentDictionary<string, EntryModel>();
        private readonly ConcurrentDictionary<string, FolderModel> _folderLookup = new ConcurrentDictionary<string, FolderModel>();

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
        }

        private void AddToSearchIndex(EntryModel entry)
        {
            var myKey = GetSearchString(entry);
            while (!_entryLookup.TryAdd(myKey, entry))
            {
                //add a character which likely won't be searched for
                myKey += "_";
            }
        }

        private void RemoveFromSearchIndex(FolderModel folder)
        {
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

        public ObservableCollection<EntryModel> SearchEntry(string searchTerm)
        {
            var list = new SortedObservableCollection<EntryModel>(new BaseModelComparator<EntryModel>());
            if (searchTerm.Length >= 2)
                Task.Run(() => SearchEntries(searchTerm, list)).ConfigureAwait(false);
            return list;
        }

        private void SearchEntries(string search, SortedObservableCollection<EntryModel> myList)
        {
            foreach (var entryLookupKey in _entryLookup.Keys)
            {
                if (
                    //if entry contains search parameter
                    entryLookupKey.Contains(search) ||

                    //if search parameter contains entry
                    search.Contains(entryLookupKey)
                )
                {
                    _dispatchService.CheckBeginInvokeOnUi(
                        () => myList.Add(_entryLookup[entryLookupKey])
                    );
                }
                //can make search arbitrary complex! try with weighting etc
            }

        }

        public ObservableCollection<FolderModel> SearchFolder(string searchTerm)
        {
            var list = new SortedObservableCollection<FolderModel>(new BaseModelComparator<FolderModel>());
            if (searchTerm.Length >= 2)
                Task.Run(() => SearchFolders(searchTerm, list)).ConfigureAwait(false);
            return list;
        }

        public FolderModel GetBestGuessParentFolder(EntryModel model)
        {
            var start = GetRootFolder();
            var res = TryFindFolderModel(start, model.ParentIds.FirstOrDefault());
            return res ?? start;
        }

        private FolderModel TryFindFolderModel(FolderModel model, Guid guid)
        {
            if (model.GetId() == guid)
            {
                return model;
            }
            foreach (var modelFolder in model.Folders)
            {
                var folder = TryFindFolderModel(modelFolder, guid);
                if (folder != null)
                {
                    return folder;
                }
            }
            return null;
        }

        private void SearchFolders(string search, SortedObservableCollection<FolderModel> myList)
        {
            foreach (var folderLookupKey in _folderLookup.Keys)
            {
                if (
                    //if entry contains search parameter
                    folderLookupKey.Contains(search) ||

                    //if search parameter contains entry
                    search.Contains(folderLookupKey)
                )
                {
                    _dispatchService.CheckBeginInvokeOnUi(
                        () => myList.Add(_folderLookup[folderLookupKey])
                    );
                }
                //can make search arbitrary complex! try with weighting etc
            }
        }
    }
}
