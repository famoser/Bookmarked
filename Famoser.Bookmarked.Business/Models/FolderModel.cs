using System.Collections.ObjectModel;
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
    }
}
