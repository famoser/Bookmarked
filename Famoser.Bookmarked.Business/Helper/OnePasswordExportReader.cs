using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Entries;
using Famoser.Bookmarked.Business.Models.Entries.Base;

namespace Famoser.Bookmarked.Business.Helper
{
    public class OnePasswordExportReader
    {
        private string _content;
        private string[][] _result;
        public OnePasswordExportReader(string content)
        {
            _content = content;
        }

        public bool Process()
        {
            var lines = _content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (!lines.Any())
                return false;

            if (lines[0] != "title,notes,username,password,url")
                return false;

            _result = new string[lines.Length - 1][];
            for (int i = 1; i < lines.Length; i++)
            {
                var row = lines[i].Split(new[] { "", "" }, StringSplitOptions.None);
                if (row.Length != 5)
                    return false;

                //remove first and last "
                row[0] = row[0].Substring(1);
                row[4] = row[4].Substring(0, row[4].Length - 1);
                _result[i - 1] = row;

                //try to contruct uri
                var uri = new Uri(row[4]);
                if (!uri.IsWellFormedOriginalString())
                    return false;
            }

            return true;
        }

        public Dictionary<string, OnlineAccountModel> GetResult()
        {
            var res = new Dictionary<string, OnlineAccountModel>();
            foreach (var row in _result)
            {
                var entryModel = new OnlineAccountModel
                {
                    WebpageUrl = new Uri(row[4]),
                    Password = row[3],
                    UserName = row[2],
                    PrivateNote = row[1]
                };
                res.Add(row[0], entryModel);
            }
            return res;
        }
    }
}
