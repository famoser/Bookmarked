using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Famoser.Bookmarked.Business.Models.Base;
using Famoser.SyncApi.Models.Interfaces;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Models
{
    public class FolderModel : ParentModel
    {
        [JsonIgnore]
        public ObservableCollection<FolderModel> Folders { get; set; } = new ObservableCollection<FolderModel>();
        
        [JsonIgnore]
        public ObservableCollection<EntryModel> Entries { get; set; } = new ObservableCollection<EntryModel>();

        public override string GetClassIdentifier()
        {
            return typeof(FolderModel).Name;
        }
    }
}
