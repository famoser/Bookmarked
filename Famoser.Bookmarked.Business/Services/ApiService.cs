using System;
using System.Linq;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Entity;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.FrameworkEssentials.Logging;
using Famoser.FrameworkEssentials.Services;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.SyncApi.Helpers;
using Famoser.SyncApi.Models;
using Famoser.SyncApi.Models.Interfaces;
using Famoser.SyncApi.Repositories.Interfaces;
using Famoser.SyncApi.Services.Interfaces;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Services
{
    public class ApiService : IApiService
    {
        private readonly SyncApiHelper _helper;

        public ApiService(IStorageService storageService, IApiTraceService apiTraceService)
        {
            _helper = new SyncApiHelper(storageService, "bookmarked_id", "https://bookmarked.syncapi.famoser.ch/")
            {
                ApiTraceService = apiTraceService
            };
        }

        public IApiRepository<T, CollectionModel> ResolveRepository<T>() where T : ISyncModel
        {
            return _helper.ResolveRepository<T>();
        }

        public async Task<Uri> GetIconUriAsync(Uri uri)
        {
            try
            {
                var apiUri = new Uri("http://icons.better-idea.org/allicons.json?url=" + uri.AbsoluteUri);
                using (var service = new HttpService())
                {
                    var resp = await service.DownloadAsync(apiUri);
                    var obj = JsonConvert.DeserializeObject<BetterIdeaIcon>(await resp.GetResponseAsStringAsync());
                    var res = obj.Icons.OrderByDescending(i => i.Width).FirstOrDefault();
                    if (res?.Url != null)
                        return new Uri(res.Url);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return null;
        }
    }
}
