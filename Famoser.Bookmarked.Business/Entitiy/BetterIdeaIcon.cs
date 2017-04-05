using System.Collections.Generic;
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
