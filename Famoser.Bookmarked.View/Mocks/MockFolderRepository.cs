using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Models.Entries.Base;
using Famoser.Bookmarked.Business.Repositories.Interfaces;

namespace Famoser.Bookmarked.View.Mocks
{
#pragma warning disable 1998
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

        public async Task<ContentModel> GetEntryContent(Entry entry)
        {
            return new WebpageModel()
            {
                WebpageUrl = new Uri("https://www.facebook.com/"),
                Icon = Convert.FromBase64String("AAABAAEAEBAAAAEAIABoBAAAFgAAACgAAAAQAAAAIAAAAAEAIAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKBgRcOjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP///////////6NiSP+jYkj/o2JI/6NiSP+iYEfEAAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj///////////+jYkj/o2JI/6NiSP+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI////////////o2JI/6NiSP+jYkj/o2JI/6NiSP8AAAAAAAAAAKNiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP///////////6NiSP+jYkj/o2JI/6NiSP+jYkj/AAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj///////////+jYkj/o2JI/6NiSP+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+CTjr/gk46////////////gk46/4JOOv+SWED/o2JI/6NiSP8AAAAAAAAAAKNiSP+jYkj/o2JI/6NiSP+jYkj/////////////////////////////////x66k/6NiSP+jYkj/AAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI//////////////////////////////////Hn4/+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI////////////ml1E/6NiSP+jYkj/o2JI/6NiSP8AAAAAAAAAAKNiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP/59vX//////6R+b/+CTjr/gk46/6NiSP+jYkj/AAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/3si///////////////////////+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6ptVv/izsb//Pv6//fx7//r3dj/o2JI/6NiSP8AAAAAAAAAAJtdRP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+bXUT/AAAAAAAAAACATTbDgk46/4JOOv+CTjr/gk46/4JOOv+CTjr/gk46/4JOOv+CTjr/gk46/4JOOv+CTjr/gE02wwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//8AAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAA//8AAA==")
            };
        }

        public Task<bool> SaveEntryAsync(Entry entry, ContentModel contentModel)
        {
            throw new NotImplementedException();
        }
    }
}
