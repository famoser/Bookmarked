using System;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Models.Entries.Base;
using Famoser.Bookmarked.Business.Repositories;
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

        public FolderModel GetRootFolder()
        {
            var fr = new FolderRepository(null, null, null);
            var rf = fr.GetRootFolder();
            var nf = fr.CreateFolder(rf);
            rf.Folders.Add(nf);
            nf.Name = "my stuff";
            nf.Description = "contains all that is well";
            var ne = fr.CreateEntry(nf, ContentType.Webpage);
            ne.Description = "hi mom";
            ne.Name = "private stuff";
            return rf;
        }

        public Task<bool> SaveFolderAsync(FolderModel folderModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFolderAsync(FolderModel folderModel)
        {
            throw new NotImplementedException();
        }

        public FolderModel CreateFolder(FolderModel parentFolderModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveEntryAsync(EntryModel entryModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveEntryAsync(EntryModel entryModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveEntryAsync(EntryModel entryModel, ContentModel contentModel)
        {
            throw new NotImplementedException();
        }

        public T GetEntryContent<T>(EntryModel entryModel) where T : ContentModel, new()
        {
            var t = new T();
            var wp = t as WebpageModel;
            wp.WebpageUrl = new Uri("https://www.facebook.com/");
            wp.Icon =
                Convert.FromBase64String(
                    "AAABAAEAEBAAAAEAIABoBAAAFgAAACgAAAAQAAAAIAAAAAEAIAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKBgRcOjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP///////////6NiSP+jYkj/o2JI/6NiSP+iYEfEAAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj///////////+jYkj/o2JI/6NiSP+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI////////////o2JI/6NiSP+jYkj/o2JI/6NiSP8AAAAAAAAAAKNiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP///////////6NiSP+jYkj/o2JI/6NiSP+jYkj/AAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj///////////+jYkj/o2JI/6NiSP+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+CTjr/gk46////////////gk46/4JOOv+SWED/o2JI/6NiSP8AAAAAAAAAAKNiSP+jYkj/o2JI/6NiSP+jYkj/////////////////////////////////x66k/6NiSP+jYkj/AAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI//////////////////////////////////Hn4/+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI////////////ml1E/6NiSP+jYkj/o2JI/6NiSP8AAAAAAAAAAKNiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP/59vX//////6R+b/+CTjr/gk46/6NiSP+jYkj/AAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/3si///////////////////////+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6ptVv/izsb//Pv6//fx7//r3dj/o2JI/6NiSP8AAAAAAAAAAJtdRP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+bXUT/AAAAAAAAAACATTbDgk46/4JOOv+CTjr/gk46/4JOOv+CTjr/gk46/4JOOv+CTjr/gk46/4JOOv+CTjr/gE02wwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//8AAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAA//8AAA==");
            return t;
        }

        public EntryModel CreateEntry(FolderModel parentFolderModel, ContentType type)
        {
            throw new NotImplementedException();
        }
    }
}
