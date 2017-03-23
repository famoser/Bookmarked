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

        private bool _isDeleted;
        internal bool IsDeleted
        {
            get { return _isDeleted; }
            set { Set(ref _isDeleted, value); }
        }
    }
}
