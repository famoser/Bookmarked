using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.View.Enum;
using Famoser.Bookmarked.View.ViewModels.Entry;
using Famoser.Bookmarked.View.ViewModels.Entry.Abstract;
using Famoser.Bookmarked.View.ViewModels.Interface;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.Bookmarked.View.Model
{
    public class ContentTypeModel
    {
        public string Name { get; set; }
        public ContentType ContentType { get; set; }
        public PageKeys AddPageKey { get; set; }
        public PageKeys EditPageKey { get; set; }
        public PageKeys ViewPageKey { get; set; }
        public Type ViewModelType { get; set; }

        internal void SetEntryToViewModel(EntryModel em, CrudState state)
        {
            var vm = SimpleIoc.Default.GetInstance(ViewModelType) as IEntryViewModel;
            vm?.SetEntry(em, state);
        }
    }
}
