using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Models.Entries.Base;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Helper
{
    public class ContentTypeHelper
    {
        public static ContentModel Deserialize(string json, ContentType type)
        {
            switch (type)
            {
                case ContentType.Webpage:
                    return JsonConvert.DeserializeObject<WebpageModel>(json);
            }
            throw new Exception("ContentType unknown");
        }
    }
}
