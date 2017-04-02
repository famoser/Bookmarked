using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.View.Enum;

namespace Famoser.Bookmarked.View.Model
{
    public class ContentTypeModel
    {
        public ContentType ContentType { get; set; }
        public string Name { get; set; }
        public Pages AddPage { get; set; }
        public Pages EditPage { get; set; }
        public Pages ViewPage { get; set; }
        internal Action<EntryModel, CrudState> SetEntryToViewModel { get; set; }
    }
}
