using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Entity;
using Famoser.Bookmarked.Business.Services.Interfaces;
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
    public class ApiService : HttpService, IApiService
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

        public async Task<string> GetWebpageNameAsync(Uri uri)
        {
            try
            {
                var content = await DownloadAsync(uri);
                const string titleRegex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";
                var ex = new Regex(titleRegex, RegexOptions.IgnoreCase);
                return ex.Match(await content.GetResponseAsStringAsync()).Value.Trim();
            }
            catch
            {
                // ignored as not critical
            }
            return null;
        }

        public IApiStorageService GetApiStorageService()
        {
            return _helper.ApiStorageService;
        }

        public Task<UserModel> GetApiUserAsync()
        {
            return _helper.ApiUserRepository.GetAsync();
        }

        public Task<bool> SetApiUserAsync(UserModel user)
        {
            return _helper.ApiUserRepository.ReplaceUserAsync(user);
        }

        public async Task<Uri> GetIconUriAsync(Uri uri)
        {
            try
            {
                if (uri != null)
                {
                    var apiUri = new Uri("http://icons.better-idea.org/allicons.json?url=" + uri.AbsoluteUri);
                    using (var service = new HttpService())
                    {
                        var resp = await service.DownloadAsync(apiUri);
                        var obj = JsonConvert.DeserializeObject<BetterIdeaIcon>(await resp.GetResponseAsStringAsync());
                        var res = obj?.Icons?.OrderByDescending(i => i.Width).FirstOrDefault();
                        if (res?.Url != null)
                            return new Uri(res.Url);
                    }
                }
            }
            catch
            {
                // ignored as not critical
            }
            return null;
        }
    }
}
