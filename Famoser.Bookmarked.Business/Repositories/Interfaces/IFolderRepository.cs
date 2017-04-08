using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries.Base;

namespace Famoser.Bookmarked.Business.Repositories.Interfaces
{
    public interface IFolderRepository
    {
        FolderModel GetRootFolder();
        FolderModel GetGarbageFolder();

        Task<bool> SyncAsnyc();

        Task<bool> SaveFolderAsync(FolderModel folderModel);
        Task<bool> MoveFolderToGarbageAsync(FolderModel folderModel);
        Task<bool> MoveFolderOutOfGarbageAsync(FolderModel folderModel);
        FolderModel CreateFolder(FolderModel parentFolderModel);
        Task<bool> RemoveFolderAsync(FolderModel folderModel);

        T GetEntryContent<T>(EntryModel entryModel) where T : ContentModel, new();
        Task<bool> SaveEntryAsync(EntryModel entryModel, ContentModel contentModel);
        Task<bool> MoveEntryToGarbageAsync(EntryModel entryModel);
        Task<bool> MoveEntryOutOfGarbageAsync(EntryModel entryModel);
        EntryModel CreateEntry(FolderModel parentFolderModel, ContentType type);
        Task<bool> RemoveEntryAsync(EntryModel entryModel);

        ObservableCollection<EntryModel> SearchEntry(string searchTerm);
    }
}
