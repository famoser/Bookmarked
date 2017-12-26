using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Famoser.Bookmarked.Business.Services.Interfaces;
using Famoser.Bookmarked.Presentation.Webpage.Services;
using Famoser.SyncApi.Models;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.Presentation.Webpage.Controllers
{
    public class ViewController : Controller
    {

        // GET: View
        public async Task<ActionResult> Navigate(Guid accountId, string path)
        {
            var bookmarkedService = new BookmarkedService(accountId);
            return View("elements", await bookmarkedService.GetContent(path));
        }
    }
}