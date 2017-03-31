using System;
using GalaSoft.MvvmLight;

namespace Famoser.Bookmarked.Business.Models.Base
{
    public class BaseModel : ObservableObject, IComparable
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

        public int CompareTo(object other)
        {
            if (other is BaseModel obj)
            {
                return String.Compare(obj.Name, Name, StringComparison.Ordinal);
            }
            return -1;
        }
    }
}
