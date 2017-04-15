using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Entries.Base;

namespace Famoser.Bookmarked.Business.Models.Entries
{
    public class NoteModel : ContentModel
    {
        public override ContentType GetContentType()
        {
            return ContentType.Note;
        }
    }
}
