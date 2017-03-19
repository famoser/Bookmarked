using Famoser.Bookmarked.Business.Services.Interfaces;

namespace Famoser.Bookmarked.Business.Services
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
