using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models.Entries;

namespace Famoser.Bookmarked.Business.Models
{
    public class ImportModel
    {
        public DateTime ExportDate { get; set; }
        public List<FolderModel> Folders { get; set; }
        public List<EntryModel> Entries { get; set; }
        public Dictionary<Guid, CreditCardModel> CreditCardModels { get; set; }
        public Dictionary<Guid, NoteModel> NoteModels { get; set; }
        public Dictionary<Guid, OnlineAccountModel> OnlineAccountModels { get; set; }
        public Dictionary<Guid, WebpageModel> WebpageModels { get; set; }
    }
}
