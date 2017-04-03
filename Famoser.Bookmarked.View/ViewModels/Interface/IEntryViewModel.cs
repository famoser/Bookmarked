using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.View.Enum;

namespace Famoser.Bookmarked.View.ViewModels.Interface
{
    internal interface IEntryViewModel
    {
        void SetEntry(EntryModel model, CrudState state);
    }
}
