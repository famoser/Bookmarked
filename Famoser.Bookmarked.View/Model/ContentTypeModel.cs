using System;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.View.Enum;

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
        public Type ModelType { get; set; }
    }
}
