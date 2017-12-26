using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.FolderRepository;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Repositories.Mocks;
using Famoser.Bookmarked.Business.Services;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.Presentation.Webpage.Platform;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Services.Interfaces;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.Presentation.Webpage.Services
{
    public class BookmarkedService
    {
        private readonly Guid _accountGuid;
        private readonly HttpServerUtilityBase _server;
        private readonly SimpleIoc _simpleIoc;
        public BookmarkedService(Guid accountGuid, SimpleIoc simpleIoc, HttpServerUtilityBase server)
        {
            _accountGuid = accountGuid;
            _server = server;
            _simpleIoc = simpleIoc;
            RegisterContainer();
        }

        private void RegisterContainer()
        {
            //locals
            _simpleIoc.Register<IDispatchService, DispatchService>();
            _simpleIoc.Register<IRandomNumberService, RandomNumberService>();

            //business
            _simpleIoc.Register<IApiService, ApiService>();
            _simpleIoc.Register<IApiTraceService, ApiTraceService>();
            _simpleIoc.Register<IEncryptionService, EncryptionService>();
            _simpleIoc.Register<IPasswordService, PasswordService>();
            _simpleIoc.Register<ISimpleProgressService, SimpleProgressService>();
            _simpleIoc.Register<IFolderRepository, FolderRepository>();
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
                        var wasInitialized = storageService.EnsureUserInitialized(_server);
                        _simpleIoc.Register<IStorageService>(() => storageService);

                        if (!wasInitialized)
                        {
                            var apiService = _simpleIoc.GetInstance<IApiService>();
                            await apiService.SetApiUserAsync(userModel);
                        }
                    });
                }
            }

            await _initTask;
            _initialized = true;
        }

        public async Task<object> GetContent(string path)
        {
            await EnsureApiInitialized();
            var folderRepo = _simpleIoc.GetInstance<IFolderRepository>();

            await folderRepo.SyncAsync();

            var activeFolder = folderRepo.GetRootFolder();
            if (path == "")
                return activeFolder;

            List<Guid> guids;
            try
            {
                if (path.Contains("_"))
                {
                    var guidStrings = path.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                    guids = guidStrings.Select(Guid.Parse).ToList();
                }
                else
                {
                    guids = new List<Guid> { Guid.Parse(path) };
                }
            }
            catch (Exception e)
            {
                //guid(s) malformed
                return null;
            }

            for (var index = 0; index < guids.Count - 1; index++)
            {
                var guid = guids[index];
                var found = false;
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
                    return null;
            }

            var lastGuid = guids.Last();
            foreach (var folder in activeFolder.Folders)
            {
                if (folder.GetId().Equals(lastGuid))
                {
                    return folder;
                }
            }

            foreach (var entry in activeFolder.Entries)
            {
                if (entry.GetId().Equals(lastGuid))
                {
                    return entry;
                }
            }

            return null;
        }
    }
}