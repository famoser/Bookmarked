using System;
using System.Threading.Tasks;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Models.Interfaces;
using Famoser.SyncApi.Repositories.Interfaces;
using Famoser.SyncApi.Services.Interfaces;

namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IApiService
    {
        IApiRepository<T, CollectionModel> ResolveRepository<T>() where T : ISyncModel;

        Task<Uri> GetIconUriAsync(Uri uri);

        Task<string> GetWebpageNameAsync(Uri uri);

        IApiStorageService GetApiStorageService();

        Task<UserModel> GetApiUserAsync();

        Task<bool> SetApiUserAsync(UserModel user);
    }
}
