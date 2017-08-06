using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;

namespace Famoser.Bookmarked.Business.Repositories.FolderRepository
{
    public partial class FolderRepository
    {
        public Task<bool> MoveFolderToGarbageAsync(FolderModel folderModel)
        {
            RemoveFolder(folderModel);
            if (!folderModel.ParentIds.Contains(_garbageGuid))
                folderModel.ParentIds.Add(_garbageGuid);
            AddFolder(folderModel);
            return _folderRepository.SaveAsync(folderModel);
        }

        public Task<bool> MoveFolderOutOfGarbageAsync(FolderModel folderModel)
        {
            RemoveFolder(folderModel);
            if (folderModel.ParentIds.Contains(_garbageGuid))
                folderModel.ParentIds.Remove(_garbageGuid);
            AddFolder(folderModel);
            return _folderRepository.SaveAsync(folderModel);
        }

        public Task<bool> MoveEntryToGarbageAsync(EntryModel entryModel)
        {
            RemoveEntry(entryModel);
            if (!entryModel.ParentIds.Contains(_garbageGuid))
                entryModel.ParentIds.Add(_garbageGuid);
            AddEntry(entryModel);
            return _entryRepository.SaveAsync(entryModel);
        }

        public Task<bool> MoveEntryOutOfGarbageAsync(EntryModel entryModel)
        {
            RemoveEntry(entryModel);
            if (entryModel.ParentIds.Contains(_garbageGuid))
                entryModel.ParentIds.Remove(_garbageGuid);
            AddEntry(entryModel);
            return _entryRepository.SaveAsync(entryModel);
        }
    }
}
