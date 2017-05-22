﻿using Famoser.Bookmarked.Business.Enum;

namespace Famoser.Bookmarked.Business.Models.Entries
{
    public class OnlineAccountModel : WebpageModel
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { Set(ref _userName, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        public override ContentType GetContentType()
        {
            return ContentType.OnlineAccount;
        }

        public void WriteProperties(WebpageModel webpage)
        {
            
        }
    }
}
