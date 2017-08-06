using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Helper;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Models.Entries.Base;

namespace Famoser.Bookmarked.Business.Repositories.FolderRepository
{
    public partial class FolderRepository
    {
        private ContentModel Upgrade(EntryModel model, ContentModel newContentModel, ContentType targetType)
        {
            if (model.ContentType == targetType)
                return null;

            ContentModel newModel = null;
            if (model.ContentType == ContentType.Note && targetType == ContentType.Webpage)
            {
                newModel = new WebpageModel();
                var nowContent = newContentModel as NoteModel;
                UpgradeHelper.WriteValues(nowContent, (WebpageModel)newModel);
            }
            if (model.ContentType == ContentType.Note && targetType == ContentType.Person)
            {
                newModel = new PersonModel();
                var nowContent = newContentModel as NoteModel;
                UpgradeHelper.WriteValues(nowContent, (PersonModel)newModel);
            }
            if (model.ContentType == ContentType.Note && targetType == ContentType.Book)
            {
                newModel = new BookModel();
                var nowContent = newContentModel as NoteModel;
                UpgradeHelper.WriteValues(nowContent, (BookModel)newModel);
            }
            if (model.ContentType == ContentType.Webpage && targetType == ContentType.CreditCard)
            {
                newModel = new CreditCardModel();
                var nowContent = newContentModel as WebpageModel;
                UpgradeHelper.WriteValues(nowContent, (CreditCardModel)newModel);
            }
            if (model.ContentType == ContentType.Webpage && targetType == ContentType.OnlineAccount)
            {
                newModel = new OnlineAccountModel();
                var nowContent = newContentModel as WebpageModel;
                UpgradeHelper.WriteValues(nowContent, (OnlineAccountModel)newModel);
            }
            if (newModel != null)
                model.ContentType = newModel.GetContentType();
            return newModel;
        }

        public Task<bool> UpgradeEntryAsync<T>(EntryModel entryModel, ContentType targetType) where T : ContentModel, new()
        {
            ContentModel newModel = GetEntryContent<T>(entryModel);
            ContentModel conversionModel = newModel;
            do
            {
                newModel = conversionModel;
                conversionModel = Upgrade(entryModel, newModel, targetType);
            } while (conversionModel != null);
            return SaveEntryAsync(entryModel, newModel);
        }
    }
}
