using Newtonsoft.Json;

namespace Famoser.Bookmarked.Presentation.Universal.Models
{
    public class PasswordInfoModel
    {
        public string AccountId { get; set; }
        public string KeyId { get; set; }
        public string KeyPhrase { get; set; }
        public string EncryptedPassword { get; set; }
        public bool PreferPassword { get; set; } = false;

        [JsonIgnore]
        public byte[] EncryptionKey { get; set; }
    }
}
