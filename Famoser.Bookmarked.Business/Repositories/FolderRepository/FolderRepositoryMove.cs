using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Extensions;
using Famoser.Bookmarked.Business.Models;

namespace Famoser.Bookmarked.Business.Repositories.FolderRepository
{
    public partial class FolderRepository
    {

        public async Task<bool> AddEntryToFolderAsync(EntryModel entryModel, FolderModel folder)
        {
            if (!folder.Entries.Contains(entryModel))
                folder.Entries.AddUniqueSorted(entryModel);
            if (!entryModel.ParentIds.Contains(folder.GetId()))
            {
                entryModel.ParentIds.Add(folder.GetId());
                return await _entryRepository.SaveAsync(entryModel);
            }
            return true;
        }

        public async Task<bool> RemoveEntryFromFolderAsync(EntryModel entryModel, FolderModel folder)
        {
            if (folder.Entries.Contains(entryModel))
                folder.Entries.Remove(entryModel);
            if (entryModel.ParentIds.Contains(folder.GetId()))
            {
                entryModel.ParentIds.Remove(folder.GetId());
                return await _entryRepository.SaveAsync(entryModel);
            }
            return true;
        }

        public async Task<bool> ReplaceFolderOfEntryAsync(EntryModel entryModel, FolderModel oldFolder, FolderModel newFolder)
        {
            bool savePls = false;
            if (!newFolder.Entries.Contains(entryModel))
                newFolder.Entries.AddUniqueSorted(entryModel);
            if (!entryModel.ParentIds.Contains(newFolder.GetId()))
            {
                entryModel.ParentIds.Add(newFolder.GetId());
                savePls = true;
            }
            if (oldFolder.Entries.Contains(entryModel))
                oldFolder.Entries.Remove(entryModel);
            if (entryModel.ParentIds.Contains(oldFolder.GetId()))
            {
                entryModel.ParentIds.Remove(oldFolder.GetId());
                savePls = true;
            }
            if (savePls)
                return await _entryRepository.SaveAsync(entryModel);
            return true;
        }

        public async Task<bool> ReplaceFolderOfEntryAsync(FolderModel folder, FolderModel oldFolder, FolderModel newFolder)
        {
            bool savePls = false;
            if (!newFolder.Folders.Contains(folder))
                newFolder.Folders.AddUniqueSorted(folder);
            if (!folder.ParentIds.Contains(newFolder.GetId()))
            {
                folder.ParentIds.Add(newFolder.GetId());
                savePls = true;
            }
            if (oldFolder.Folders.Contains(folder))
                oldFolder.Folders.Remove(folder);
            if (folder.ParentIds.Contains(oldFolder.GetId()))
            {
                folder.ParentIds.Remove(oldFolder.GetId());
                savePls = true;
            }
            if (savePls)
                return await _folderRepository.SaveAsync(folder);
            return true;
        }
    }
}
