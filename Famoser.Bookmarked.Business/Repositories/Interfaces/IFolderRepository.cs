using System.Collections.ObjectModel;
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

        Task<bool> SyncAsync();

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
        Task<bool> AddEntryToFolderAsync(EntryModel entryModel, FolderModel folder);
        Task<bool> RemoveEntryFromFolderAsync(EntryModel entryModel, FolderModel folder);
        Task<bool> ReplaceFolderOfEntryAsync(EntryModel entryModel, FolderModel oldFolder, FolderModel newFolder);
        Task<bool> ReplaceFolderOfEntryAsync(FolderModel folder, FolderModel oldFolder, FolderModel newFolder);

        ObservableCollection<EntryModel> SearchEntry(string searchTerm);
        ObservableCollection<FolderModel> SearchFolder(string searchTerm);
        Task<string> ExportDataAsync();
        Task<bool> ImportDataAsync(string content);
        Task<string> ExportCredentialsAsync();
        Task<bool> ImportCredentialsAsync(string content);
        Task<bool> ClearAllDataAsync();
        Task<bool> UpgradeEntryAsync<T>(EntryModel entryModel, ContentType target) where T : ContentModel, new();
    }
}
