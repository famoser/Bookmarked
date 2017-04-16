using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.ViewModels.Entry;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.View.Helper
{
    public class ContentHelper
    {
        private static readonly List<ContentTypeModel> ContentTypeModels = new List<ContentTypeModel>()
        {
            new ContentTypeModel()
            {
                Name = "Webpage",
                ContentType = ContentType.Webpage,
                AddPageKey = PageKeys.AddWebpage,
                EditPageKey = PageKeys.EditWebpage,
                ViewPageKey = PageKeys.ViewWebpage,
                ViewModelType = typeof(WebpageViewModel),
                ModelType = typeof(WebpageModel)
            },
            new ContentTypeModel()
            {
                Name = "Online Account",
                ContentType = ContentType.OnlineAccount,
                AddPageKey = PageKeys.AddOnlineAccount,
                EditPageKey = PageKeys.EditOnlineAccount,
                ViewPageKey = PageKeys.ViewOnlineAccount,
                ViewModelType = typeof(OnlineAccountViewModel),
                ModelType = typeof(OnlineAccountModel)
            },
            new ContentTypeModel()
            {
                Name = "Note",
                ContentType = ContentType.Note,
                AddPageKey = PageKeys.AddNote,
                EditPageKey = PageKeys.EditNote,
                ViewPageKey = PageKeys.ViewNote,
                ViewModelType = typeof(NoteViewModel),
                ModelType = typeof(NoteModel)
            },
            new ContentTypeModel()
            {
                Name = "Credit Card",
                ContentType = ContentType.CreditCard,
                AddPageKey = PageKeys.AddCreditCard,
                EditPageKey = PageKeys.EditCreditCard,
                ViewPageKey = PageKeys.ViewCreditCard,
                ViewModelType = typeof(CreditCardViewModel),
                ModelType = typeof(CreditCardModel)
            }
        };

        public static List<ContentTypeModel> GetContentTypeModels()
        {
            return ContentTypeModels;
        }

        private static Dictionary<ContentType, ContentTypeModel> _contentTypeDictionary;
        public static ContentTypeModel GetContentTypeModel(ContentType type)
        {
            if (_contentTypeDictionary == null)
            {
                _contentTypeDictionary = new Dictionary<ContentType, ContentTypeModel>();
                foreach (var contentTypeModel in ContentTypeModels)
                {
                    _contentTypeDictionary.Add(contentTypeModel.ContentType, contentTypeModel);
                }
            }
            if (_contentTypeDictionary.ContainsKey(type))
                return _contentTypeDictionary[type];
            return null;
        }

        private static Dictionary<Type, ContentTypeModel> _modelTypeDictionary;
        public static ContentTypeModel GetContentTypeModel(Type type)
        {
            if (_modelTypeDictionary == null)
            {
                _modelTypeDictionary = new Dictionary<Type, ContentTypeModel>();
                foreach (var contentTypeModel in ContentTypeModels)
                {
                    _modelTypeDictionary.Add(contentTypeModel.ModelType, contentTypeModel);
                }
            }
            if (_modelTypeDictionary.ContainsKey(type))
                return _modelTypeDictionary[type];
            return null;
        }
    }
}
