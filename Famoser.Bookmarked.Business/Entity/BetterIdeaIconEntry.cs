using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Entity
{
    internal class BetterIdeaIconEntry
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("format")]
        public string Format { get; set; }
        [JsonProperty("bytes")]
        public int Bytes { get; set; }
        [JsonProperty("error")]
        public object Error { get; set; }
        [JsonProperty("sha1sum")]
        public string Sha1Sum { get; set; }
    }
}
