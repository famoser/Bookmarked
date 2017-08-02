using System.Collections.ObjectModel;
using System.Linq;
using Famoser.Bookmarked.Business.Models.Base;
using Newtonsoft.Json;

namespace Famoser.Bookmarked.Business.Models
{
    public class FolderModel : ParentModel
    {
        [JsonIgnore]
        public ObservableCollection<FolderModel> Folders { get; } = new ObservableCollection<FolderModel>();
        
        [JsonIgnore]
        public ObservableCollection<EntryModel> Entries { get; } = new ObservableCollection<EntryModel>();

        public override string GetClassIdentifier()
        {
            return typeof(FolderModel).Name;
        }

        public int TotalFoldersInStructure => Folders.Sum(f => f.TotalFoldersInStructure) + Folders.Count;
        public int TotalEntriesInStructure => Folders.Sum(f => f.TotalEntriesInStructure) + Entries.Count;
    }
}
