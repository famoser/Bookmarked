using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Entitiy
{
    internal class BetterIdeaIcon
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("icons")]
        public List<BetterIdeaIconEntry> Icons { get; set; }
    }
}
