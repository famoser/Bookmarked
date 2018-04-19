using System;
using System.Collections.Generic;
using System.Linq;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.ViewModels.Entry;

namespace Famoser.Bookmarked.View.Helper
{
    public class ContentHelper
    {
        private static readonly Dictionary<ContentType, ContentTypeModel> ContentTypeModels = new Dictionary<ContentType, ContentTypeModel>
        {
            {
                ContentType.Note,
                new ContentTypeModel
                {
                    Name = "Note",
                    ContentType = ContentType.Note,
                    AddPageKey = PageKeys.AddNote,
                    EditPageKey = PageKeys.EditNote,
                    ViewPageKey = PageKeys.ViewNote,
                    ViewModelType = typeof(NoteViewModel),
                    ModelType = typeof(NoteModel)
                }
            },
            {
                ContentType.Webpage,
                new ContentTypeModel
                {
                    Name = "Webpage",
                    ContentType = ContentType.Webpage,
                    AddPageKey = PageKeys.AddWebpage,
                    EditPageKey = PageKeys.EditWebpage,
                    ViewPageKey = PageKeys.ViewWebpage,
                    ViewModelType = typeof(WebpageViewModel),
                    ModelType = typeof(WebpageModel)
                }
            },
            {
                ContentType.OnlineAccount,
                new ContentTypeModel
                {
                    Name = "Online Account",
                    ContentType = ContentType.OnlineAccount,
                    AddPageKey = PageKeys.AddOnlineAccount,
                    EditPageKey = PageKeys.EditOnlineAccount,
                    ViewPageKey = PageKeys.ViewOnlineAccount,
                    ViewModelType = typeof(OnlineAccountViewModel),
                    ModelType = typeof(OnlineAccountModel)
                }
            },
            {
                ContentType.CreditCard,
                new ContentTypeModel
                {
                    Name = "Number Collection",
                    ContentType = ContentType.CreditCard,
                    AddPageKey = PageKeys.AddCreditCard,
                    EditPageKey = PageKeys.EditCreditCard,
                    ViewPageKey = PageKeys.ViewCreditCard,
                    ViewModelType = typeof(CreditCardViewModel),
                    ModelType = typeof(CreditCardModel)
                }
            },
            {
                ContentType.Book,
                new ContentTypeModel
                {
                    Name = "Book",
                    ContentType = ContentType.Book,
                    AddPageKey = PageKeys.AddBook,
                    EditPageKey = PageKeys.EditBook,
                    ViewPageKey = PageKeys.ViewBook,
                    ViewModelType = typeof(BookViewModel),
                    ModelType = typeof(BookModel)
                }
            },
            {
                ContentType.Person,
                new ContentTypeModel
                {
                    Name = "Person",
                    ContentType = ContentType.Person,
                    AddPageKey = PageKeys.AddPerson,
                    EditPageKey = PageKeys.EditPerson,
                    ViewPageKey = PageKeys.ViewPerson,
                    ViewModelType = typeof(PersonViewModel),
                    ModelType = typeof(PersonModel)
                }
            }
        };

        public static List<ContentTypeModel> GetContentTypeModels()
        {
            return ContentTypeModels.Values.ToList();
        }

        private static Dictionary<ContentType, ContentTypeModel> _contentTypeDictionary;
        public static ContentTypeModel GetContentTypeModel(ContentType type)
        {
            if (_contentTypeDictionary == null)
            {
                _contentTypeDictionary = new Dictionary<ContentType, ContentTypeModel>();
                foreach (var contentTypeModel in ContentTypeModels.Values)
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
                foreach (var contentTypeModel in ContentTypeModels.Values)
                {
                    _modelTypeDictionary.Add(contentTypeModel.ModelType, contentTypeModel);
                }
            }
            if (_modelTypeDictionary.ContainsKey(type))
                return _modelTypeDictionary[type];
            return null;
        }

        public static List<ContentTypeModel> GetCanUpgradeContentTypes(Type type)
        {
            var ct = GetContentTypeModel(type);
            List<ContentType> typeList;
            switch (ct.ContentType)
            {
                case ContentType.Note:
                    typeList = new List<ContentType> { ContentType.Webpage, ContentType.OnlineAccount, ContentType.CreditCard, ContentType.Book, ContentType.Person };
                    break;
                case ContentType.Webpage:
                    typeList = new List<ContentType> { ContentType.OnlineAccount, ContentType.CreditCard };
                    break;
                default:
                    typeList = new List<ContentType>();
                    break;
            }
            var resList = new List<ContentTypeModel>();
            foreach (var contentType in typeList)
            {
                resList.Add(ContentTypeModels[contentType]);
            }
            return resList;
        }
    }
}
