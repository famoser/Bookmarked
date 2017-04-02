﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.ViewModels.Entry;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.View.Helper
{
    public class ContentHelper
    {
        public static List<ContentTypeModel> GetContentTypeModels()
        {
            var list = new List<ContentTypeModel>
            {
                WebpageContentTypeModel
            };
            return list;
        }

        private static readonly ContentTypeModel WebpageContentTypeModel = new ContentTypeModel()
        {
            Name = "Webpage",
            ContentType = ContentType.Webpage,
            AddPage = Pages.AddWebpage,
            EditPage = Pages.EditWebpage,
            ViewPage = Pages.ViewWebpage,
            SetEntryToViewModel = SimpleIoc.Default.GetInstance<WebpageViewModel>().SetEntry
        };
        public static ContentTypeModel GetWebpageContentTypeModel()
        {
            return WebpageContentTypeModel;
        }
    }
}
