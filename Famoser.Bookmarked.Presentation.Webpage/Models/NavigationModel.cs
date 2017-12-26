using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Famoser.Bookmarked.Business.Models;

namespace Famoser.Bookmarked.Presentation.Webpage.Models
{
    public class NavigationModel
    {
        public string Path { get; set; }

        public string BackPath
        {
            get
            {
                if (Path != "/")
                {
                    return Path.Substring(0, Path.LastIndexOf("/", StringComparison.Ordinal));
                }

                return Path;
            }
        }
        public FolderModel FolderModel { get; set; }
        public EntryModel EntryModel { get; set; }
        public bool IsFolderView { get; set; }
        public bool IsNotFoundView { get; set; }
        public Guid AccountId { get; set; }
    }
}