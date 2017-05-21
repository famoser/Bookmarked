using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Models.Entries.Base;
using Famoser.Bookmarked.Business.Repositories.Interfaces;
using PCLCrypto;

namespace Famoser.Bookmarked.Business.Helper
{
    public class UpgradeHelper
    {
        public static void WriteValues(NoteModel source, WebpageModel target)
        {
            target.PrivateNote = source.PrivateNote;
        }
        public static void WriteValues(WebpageModel source, OnlineAccountModel target)
        {
            WriteValues(source as NoteModel, target);
            target.WebpageUrl = source.WebpageUrl;
        }
    }
}
