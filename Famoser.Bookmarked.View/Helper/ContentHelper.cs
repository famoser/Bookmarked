using System;
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
                WebpageContentTypeModel,
                NoteContentTypeModel,
                OnlineAccountContentTypeModel,
                CreditCardContentTypeModel
            };
            return list;
        }

        private static readonly ContentTypeModel WebpageContentTypeModel = new ContentTypeModel()
        {
            Name = "Webpage",
            ContentType = ContentType.Webpage,
            AddPageKey = PageKeys.AddWebpage,
            EditPageKey = PageKeys.EditWebpage,
            ViewPageKey = PageKeys.ViewWebpage,
            ViewModelType = typeof(WebpageViewModel)
        };

        private static readonly ContentTypeModel OnlineAccountContentTypeModel = new ContentTypeModel()
        {
            Name = "Online Account",
            ContentType = ContentType.OnlineAccount,
            AddPageKey = PageKeys.AddOnlineAccount,
            EditPageKey = PageKeys.EditOnlineAccount,
            ViewPageKey = PageKeys.ViewOnlineAccount,
            ViewModelType = typeof(OnlineAccountViewModel)
        };

        private static readonly ContentTypeModel NoteContentTypeModel = new ContentTypeModel()
        {
            Name = "Note",
            ContentType = ContentType.Note,
            AddPageKey = PageKeys.AddNote,
            EditPageKey = PageKeys.EditNote,
            ViewPageKey = PageKeys.ViewNote,
            ViewModelType = typeof(NoteViewModel)
        };

        private static readonly ContentTypeModel CreditCardContentTypeModel = new ContentTypeModel()
        {
            Name = "Credit Card",
            ContentType = ContentType.CreditCard,
            AddPageKey = PageKeys.AddCreditCard,
            EditPageKey = PageKeys.EditCreditCard,
            ViewPageKey = PageKeys.ViewCreditCard,
            ViewModelType = typeof(CreditCardViewModel)
        };

        public static ContentTypeModel GetWebpageContentTypeModel()
        {
            return WebpageContentTypeModel;
        }

        public static ContentTypeModel GetNoteContentTypeModel()
        {
            return NoteContentTypeModel;
        }

        public static ContentTypeModel GetOnlineAccountContentTypeModel()
        {
            return OnlineAccountContentTypeModel;
        }

        public static ContentTypeModel GetCreditCardContentTypeModel()
        {
            return CreditCardContentTypeModel;
        }
    }
}
