using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries.Base;

namespace Famoser.Bookmarked.Business.Repositories.Interfaces
{
    public interface IFolderRepository
    {
        FolderModel GetRootFolder();

        Task<bool> SyncAsnyc();

        Task<bool> SaveFolderAsync(FolderModel folderModel);
        Task<bool> RemoveFolderAsync(FolderModel folderModel);
        FolderModel CreateFolderAsync(FolderModel parentFolderModel);

        Task<T> GetEntryContent<T>(EntryModel entryModel) where T : ContentModel, new();
        Task<bool> SaveEntryAsync(EntryModel entryModel, ContentModel contentModel);
        Task<bool> RemoveEntryAsync(EntryModel entryModel);
        EntryModel CreateEntryAsync(FolderModel parentFolderModel);
    }
}
