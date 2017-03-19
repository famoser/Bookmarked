using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.LectureSync.Business.Services.Interfaces;
using Famoser.SyncApi.Helpers;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Models.Interfaces;
using Famoser.SyncApi.Repositories.Interfaces;
using Famoser.SyncApi.Services.Interfaces;

namespace Famoser.LectureSync.Business.Services
{
    public class ApiService : IApiService
    {
        private readonly SyncApiHelper _helper;
        public ApiService(IStorageService storageService, IApiTraceService apiTraceService)
        {
            _helper = new SyncApiHelper(storageService, "study_id", "https://syncapi.famoser.ch/")
            {
                ApiTraceService = apiTraceService
            };
        }

        public IApiRepository<T, CollectionModel> ResolveRepository<T>() where T : ISyncModel
        {
            return _helper.ResolveRepository<T>();
        }
    }
}
