using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries.Base;

namespace Famoser.Bookmarked.Business.Repositories.Interfaces
{
    public interface IFolderRepository
    {
        Folder GetRootFolder();

        Task<bool> SyncAsnyc();

        Task<bool> SaveFolderAsync(Folder folder);
        Task<bool> RemoveFolderAsync(Folder folder);
        Folder CreateFolderAsync(Folder parentFolder);

        Task<ContentModel> GetEntryContent(Entry entry);
        Task<bool> SaveEntryAsync(Entry entry, ContentModel contentModel);
        Task<bool> RemoveEntryAsync(Entry entry);
        Entry CreateEntryAsync(Folder parentFolder);
    }
}
