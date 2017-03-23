using System;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Models.Base
{
    public class BaseModel : ObservableObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
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
        internal virtual void OnSerializingMethod(StreamingContext context)
        {
            Base64Image = Convert.ToBase64String(Image);
        }

        [OnDeserialized]
        internal virtual void OnDeserializedMethod(StreamingContext context)
        {
            Image = Convert.FromBase64String(Base64Image);
        }
    }
}
