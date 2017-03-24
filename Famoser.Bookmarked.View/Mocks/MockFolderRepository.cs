using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;

namespace Famoser.Bookmarked.View.Mocks
{
    internal class MockFolderRepository : IFolderRepository
    {
        public Task<bool> SyncAsnyc()
        {
            throw new NotImplementedException();
        }

        public Folder GetRootFolder()
        {
            return new Folder()
            {
                Folders = new ObservableCollection<Folder>()
                {
                    new Folder()
                    {
                        Name = "my folder",
                        Description = "small utils"
                    },
                    new Folder()
                    {
                        Name = "my folder 2",
                        Description = "sutff utils"
                    },
                    new Folder()
                    {
                        Name = "banana pics",
                        Description = "all abourd the banana train"
                    }
                },
                Entries = new ObservableCollection<Entry>()
                {
                    new Entry()
                    {
                        Name = "stuff",
                        Description = "content"
                    }
                }
            };
        }

        public Task<bool> SaveFolderAsync(Folder folder)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFolderAsync(Folder folder)
        {
            throw new NotImplementedException();
        }

        public Folder CreateFolderAsync(Folder parentFolder)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveEntryAsync(Entry entry)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveEntryAsync(Entry entry)
        {
            throw new NotImplementedException();
        }

        public Entry CreateEntryAsync(Folder parentFolder)
        {
            throw new NotImplementedException();
        }
    }
}
