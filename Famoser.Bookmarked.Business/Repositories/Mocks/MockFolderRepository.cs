﻿using System;
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
        public async Task<bool> SyncAsnyc()
        {
            return true;
        }

        public FolderModel GetRootFolder()
        {
            var rf = new FolderModel();
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

        public async Task<bool> SaveEntryAsync(EntryModel entryModel)
        {
            return true;
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
    }
}
