using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Models.Entries.Base;
using Famoser.Bookmarked.Business.Repositories.Interfaces;

namespace Famoser.Bookmarked.Business.Repositories.Mocks
{
#pragma warning disable 1998
    public class MockFolderRepository : IFolderRepository
    {
        public async Task<bool> SyncAsync()
        {
            return true;
        }

        private FolderModel CreateFolder(string name, string description)
        {
            var rf = new FolderModel()
            {
                Name = name, Description = description
            };
            var nf = new FolderModel();
            rf.Folders.Add(nf);
            nf.Name = "my stuff";
            nf.Description = "contains all that is well";
            rf.Folders.Add(nf);
            nf.Name = "my stuff 2";
            nf.Description = "contains all that is well 2";
            var ne = new EntryModel
            {
                ContentType = ContentType.Webpage,
                Description = "hi mom",
                Name = "private stuff",
                IconUri = new Uri("https://www.facebook.com/favicon.ico")
            };
            rf.Entries.Add(ne);
            ne = new EntryModel
            {
                ContentType = ContentType.Webpage,
                Description = "hi mom 2",
                Name = "private stuff 2",
                IconUri = new Uri("https://www.facebook.com/favicon.ico")
            };
            rf.Entries.Add(ne);
            return rf;
        }
        public FolderModel GetRootFolder()
        {
            return CreateFolder("root", "this is the root folder");
        }

        public async Task<bool> SaveFolderAsync(FolderModel folderModel)
        {
            return true;
        }

        public async Task<bool> MoveFolderToGarbageAsync(FolderModel folderModel)
        {
            return true;
        }

        public FolderModel CreateFolder(FolderModel parentFolderModel)
        {
            return new FolderModel();
        }

        public async Task<bool> MoveEntryToGarbageAsync(EntryModel entryModel)
        {
            return true;
        }

        public async Task<bool> SaveEntryAsync(EntryModel entryModel, ContentModel contentModel)
        {
            return true;
        }

        public T GetEntryContent<T>(EntryModel entryModel) where T : ContentModel, new()
        {
            var t = new T();
            var wp = t as WebpageModel;
            wp.WebpageUrl = new Uri("https://www.facebook.com/");
            return t;
        }

        public EntryModel CreateEntry(FolderModel parentFolderModel, ContentType type)
        {
            return new EntryModel();
        }

        public async Task<bool> MoveFolderOutOfGarbageAsync(FolderModel folderModel)
        {
            return true;
        }

        public async Task<bool> RemoveFolderAsync(FolderModel folderModel)
        {
            return true;
        }

        public async Task<bool> MoveEntryOutOfGarbageAsync(EntryModel entryModel)
        {
            return true;
        }

        public async Task<bool> RemoveEntryAsync(EntryModel entryModel)
        {
            return true;
        }

        public async Task<bool> AddEntryToFolderAsync(EntryModel entryModel, FolderModel folder)
        {
            return true;
        }

        public async Task<bool> RemoveEntryFromFolderAsync(EntryModel entryModel, FolderModel folder)
        {
            return true;
        }

        public async Task<bool> ReplaceFolderOfEntryAsync(EntryModel entryModel, FolderModel oldFolder, FolderModel newFolder)
        {
            return true;
        }

        public async Task<bool> ReplaceFolderOfEntryAsync(FolderModel folder, FolderModel oldFolder, FolderModel newFolder)
        {
            return true;
        }

        public FolderModel GetGarbageFolder()
        {
            return CreateFolder("garbage", "this is the garbage folder");
        }

        public ObservableCollection<EntryModel> SearchEntry(string searchTerm)
        {
            return GetRootFolder().Entries;
        }

        public ObservableCollection<FolderModel> SearchFolder(string searchTerm)
        {
            return GetRootFolder().Folders;
        }

        public async Task<string> ExportDataAsync()
        {
            return "hi mom";
        }

        public async Task<bool> ImportDataAsync(string content)
        {
            return true;
        }

        public async Task<string> GetImportDataTemplateAsync()
        {
            return "";
        }

        public async Task<string> ExportCredentialsAsync()
        {
            return "hi dad";
        }

        public async Task<bool> ImportCredentialsAsync(string content, string password)
        {
            return true;
        }

        public async Task<bool> ClearAllDataAsync()
        {
            return true;
        }

        public async  Task<bool> UpgradeEntryAsync<T>(EntryModel entryModel, ContentType target) where T : ContentModel, new()
        {
            return true;
        }
    }
}
