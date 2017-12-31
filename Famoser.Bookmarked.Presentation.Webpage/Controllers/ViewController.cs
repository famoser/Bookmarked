using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.Presentation.Webpage.Models;
using Famoser.Bookmarked.Presentation.Webpage.Services;
using Famoser.SyncApi.Models;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.Presentation.Webpage.Controllers
{
    public class ViewController : Controller
    {
        private SimpleIoc _activeIoc;

        private Guid _activeGuid;

        private SimpleIoc GetIocForGuid(Guid accountGuid)
        {
            if (accountGuid == _activeGuid)
            {
                return _activeIoc;
            }

            _activeGuid = accountGuid;
            _activeIoc = new SimpleIoc();
            return _activeIoc;
        }

        // GET: View
        public async Task<ActionResult> Navigate(Guid accountId, string path = "")
        {
            var bookmarkedService = new BookmarkedService(accountId, GetIocForGuid(accountId), Server);
            var item = await bookmarkedService.GetContent(path);


            var model = new NavigationModel { Path = path, AccountId = accountId };
            if (item == null)
            {
                model.IsNotFoundView = true;
            }
            else if (item is FolderModel folderModel)
            {
                model.FolderModel = folderModel;
                model.IsFolderView = true;
            }
            else
            {
                model.EntryModel = (EntryModel)item;
            }

            return View("Navigate", model);
        }
    }
}