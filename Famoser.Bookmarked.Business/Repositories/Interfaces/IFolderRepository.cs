using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;

namespace Famoser.Bookmarked.Business.Repositories.Interfaces
{
    public interface IFolderRepository
    {
        Folder GetRootFolder();

        Task<bool> SyncAsnyc();

        Task<bool> SaveFolderAsync(Folder folder);
        Task<bool> RemoveFolderAsync(Folder folder);
        Folder CreateFolderAsync(Folder parentFolder);

        Task<bool> SaveEntryAsync(Entry entry);
        Task<bool> RemoveEntryAsync(Entry entry);
        Entry CreateEntryAsync(Folder parentFolder);
    }
}
