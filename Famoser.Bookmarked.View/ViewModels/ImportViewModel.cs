using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Helper;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Base;
using Famoser.FrameworkEssentials.Logging;

namespace Famoser.Bookmarked.View.ViewModels
{
    public class ImportViewModel : BaseViewModel
    {
        private IFolderRepository _folderRepository;
        private IApiService _apiService;

        public ImportViewModel(IFolderRepository folderRepository, IApiService apiService)
        {
            _folderRepository = folderRepository;
            _apiService = apiService;
        }

        public async Task<bool> Import1Password(string file)
        {
            var helper = new OnePasswordExportReader(file);
            var root = _folderRepository.GetRootFolder();
            if (helper.Process())
            {
                foreach (var contentModel in helper.GetResult())
                {
                    try
                    {
                        var entryModel = _folderRepository.CreateEntry(root, ContentType.OnlineAccount);
                        entryModel.ContentType = ContentType.Webpage;
                        entryModel.IconUri = await _apiService.GetIconUriAsync(contentModel.Value.WebpageUrl);
                        entryModel.Name = contentModel.Key;
                        await _folderRepository.SaveEntryAsync(entryModel, contentModel.Value);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Instance.LogException(e, this);
                    }
                }
            }
            return true;
        }
    }
}
