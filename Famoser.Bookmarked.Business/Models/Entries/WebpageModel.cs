using System;
using System.Runtime.Serialization;
using Famoser.Bookmarked.Business.Enum;
using Famoser.Bookmarked.Business.Models.Entries.Base;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Models.Entries
{
    public class WebpageModel : ContentModel
    {
        private Uri _webpageUrl;
        public Uri WebpageUrl
        {
            get { return _webpageUrl; }
            set { Set(ref _webpageUrl, value); }
        }

        private string _image;
        internal string Image
        {
            get { return _image; }
            set { Set(ref _image, value); }
        }

        private byte[] _icon;
        [JsonIgnore]
        public byte[] Icon
        {
            get { return _icon; }
            set { Set(ref _icon, value); }
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            Image = Convert.ToBase64String(Icon);
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Icon = Convert.FromBase64String(Image);
        }

        public override ContentType GetContentType()
        {
            return ContentType.Webpage;
        }
    }
}
