using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Famoser.Bookmarked.Business.Models.Base;
using Famoser.SyncApi.Models.Interfaces;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Models
{
    public class Entry : ParentModel
    {
        private Uri _webpageUrl;
        public Uri WebpageUrl
        {
            get { return _webpageUrl; }
            set { Set(ref _webpageUrl, value); }
        }
        
        private string _base64Image;
        internal string Base64Image
        {
            get { return _base64Image; }
            set { Set(ref _base64Image, value); }
        }

        private byte[] _image;
        [JsonIgnore]
        public byte[] Image
        {
            get { return _image; }
            set { Set(ref _image, value); }
        }

        [OnSerializing]
        internal override void OnSerializingMethod(StreamingContext context)
        {
            Base64Image = Convert.ToBase64String(Image);
            base.OnSerializingMethod(context);
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Image = Convert.FromBase64String(Base64Image);
        }

        #region SyncApi implementation
        public override string GetClassIdentifier()
        {
            return typeof(Entry).Name;
        }
        #endregion
    }
}
