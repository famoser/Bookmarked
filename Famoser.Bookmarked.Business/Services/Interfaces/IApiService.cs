using Famoser.SyncApi.Models;
using Famoser.SyncApi.Models.Interfaces;
using Famoser.SyncApi.Repositories.Interfaces;

namespace Famoser.LectureSync.Business.Services.Interfaces
{
    public interface IApiService
    {
        IApiRepository<T, CollectionModel> ResolveRepository<T>() where T : ISyncModel;
    }
}
