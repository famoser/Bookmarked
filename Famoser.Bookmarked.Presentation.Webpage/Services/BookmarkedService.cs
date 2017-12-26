using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.Presentation.Webpage.Platform;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.SyncApi.Models;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.Presentation.Webpage.Services
{
    public class BookmarkedService
    {
        private readonly Guid _accountGuid;
        public BookmarkedService(Guid accountGuid)
        {
            _accountGuid = accountGuid;
            RegisterContainer();
        }

        private void RegisterContainer()
        {
            SimpleIoc.Default.Register<IDispatchService, DispatchService>();
            SimpleIoc.Default.Register<IRandomNumberService, RandomNumberService>();
        }

        private bool _initialized;
        private Task _initTask;

        private async Task EnsureApiInitialized()
        {
            lock (this)
            {
                if (_initialized)
                    return;

                if (_initTask == null)
                {
                    _initTask = Task.Run(async () =>
                    {
                        var userModel = new UserModel();
                        userModel.SetId(_accountGuid);

                        var storageService = new StorageService(_accountGuid);
                        var wasInitialized = storageService.EnsureUserInitialized();
                        SimpleIoc.Default.Register<IStorageService>(() => storageService);

                        if (!wasInitialized)
                        {
                            var apiService = SimpleIoc.Default.GetInstance<IApiService>();
                            await apiService.SetApiUserAsync(userModel);
                        }
                    });
                }
            }

            await _initTask;
            _initialized = true;
        }

        public async Task<FolderModel> GetContent(string path)
        {
            await EnsureApiInitialized();
            var folderRepo = SimpleIoc.Default.GetInstance<IFolderRepository>();

            await folderRepo.SyncAsync();

            var activeFolder = folderRepo.GetRootFolder();
            if (path != "")
            {
                var guidStrings = path.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
                var guids = guidStrings.Select(Guid.Parse).ToList();
                foreach (var guid in guids)
                {
                    bool found = false;
                    foreach (var folder in activeFolder.Folders)
                    {
                        if (folder.GetId().Equals(guid))
                        {
                            activeFolder = folder;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        return null;
                    }
                }
            }
            
            return activeFolder;
        }
    }
}