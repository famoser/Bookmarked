using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Repositories.Interfaces;

namespace Famoser.Bookmarked.Business.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly IApiRepository<Folder, CollectionModel> _folderRepository;
        private readonly IApiRepository<Entry, CollectionModel> _entryRepository;

        public FolderRepository(IApiService apiService)
        {
            _folderRepository = apiService.ResolveRepository<Folder>();
            _entryRepository = apiService.ResolveRepository<Entry>();
        }

        private ObservableCollection<Folder> _folders;
        private ObservableCollection<Folder> _folderDic;
        private Folder _root;

        public Folder GetRootFolder()
        {
            return _root;
        }

        public async Task<bool> SyncAsnyc()
        {
            var res = await _folderRepository.SyncAsync();
            //enforce both try to sync
            return await _entryRepository.SyncAsync() && res;
        }

        public Task<bool> SaveFolderAsync(Folder folder)
        {
            return _folderRepository.SaveAsync(folder);
        }

        public Task<bool> RemoveFolderAsync(Folder folder)
        {
            folder.IsDeleted = true;
            folder.Parent.Children.Remove(folder);
            return _folderRepository.RemoveAsync(folder);
        }

        public Folder CreateFolderAsync(Folder parentFolder)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveEntryAsync(Entry entry)
        {
            return _folderRepository.SaveAsync(entry);
        }

        public Task<bool> RemoveEntryAsync(Entry entry)
        {
            entry.IsDeleted = true;
            entry.Parent.Children.Remove(entry);
            return _entryRepository.RemoveAsync(entry);
        }

        public Entry CreateEntryAsync(Folder parentFolder)
        {
            throw new System.NotImplementedException();
        }
    }
}
