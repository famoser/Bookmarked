using System;
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
                Name = "private stuff"
            };
            rf.Entries.Add(ne);
            ne = new EntryModel
            {
                ContentType = ContentType.Webpage,
                Description = "hi mom 2",
                Name = "private stuff 2"
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
            wp.Icon =
                Convert.FromBase64String(
                    "AAABAAEAEBAAAAEAIABoBAAAFgAAACgAAAAQAAAAIAAAAAEAIAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKBgRcOjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP///////////6NiSP+jYkj/o2JI/6NiSP+iYEfEAAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj///////////+jYkj/o2JI/6NiSP+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI////////////o2JI/6NiSP+jYkj/o2JI/6NiSP8AAAAAAAAAAKNiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP///////////6NiSP+jYkj/o2JI/6NiSP+jYkj/AAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj///////////+jYkj/o2JI/6NiSP+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+CTjr/gk46////////////gk46/4JOOv+SWED/o2JI/6NiSP8AAAAAAAAAAKNiSP+jYkj/o2JI/6NiSP+jYkj/////////////////////////////////x66k/6NiSP+jYkj/AAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI//////////////////////////////////Hn4/+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI////////////ml1E/6NiSP+jYkj/o2JI/6NiSP8AAAAAAAAAAKNiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP/59vX//////6R+b/+CTjr/gk46/6NiSP+jYkj/AAAAAAAAAACjYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/3si///////////////////////+jYkj/o2JI/wAAAAAAAAAAo2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6ptVv/izsb//Pv6//fx7//r3dj/o2JI/6NiSP8AAAAAAAAAAJtdRP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+jYkj/o2JI/6NiSP+bXUT/AAAAAAAAAACATTbDgk46/4JOOv+CTjr/gk46/4JOOv+CTjr/gk46/4JOOv+CTjr/gk46/4JOOv+CTjr/gE02wwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//8AAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAA//8AAA==");
            return t;
        }

        public EntryModel CreateEntry(FolderModel parentFolderModel, ContentType type)
        {
            return new EntryModel();
        }
    }
}
