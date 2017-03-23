using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Famoser.Bookmarked.Business.Models.Base;
using Famoser.SyncApi.Models.Interfaces;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Models
{
    public class Folder : ParentModel
    {
        [JsonIgnore]
        public ObservableCollection<Folder> Folders { get; set; } = new ObservableCollection<Folder>();
        
        [JsonIgnore]
        public ObservableCollection<Entry> Entries { get; set; } = new ObservableCollection<Entry>();

        public override string GetClassIdentifier()
        {
            return typeof(Folder).Name;
        }
    }
}
