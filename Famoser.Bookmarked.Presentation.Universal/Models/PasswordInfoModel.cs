namespace Famoser.Bookmarked.Presentation.Universal.Models
{
    public class PasswordInfoModel
    {
        public string KeyPhrase { get; set; }
        public string EncryptedPassword { get; set; }
        public bool PreferPassword { get; set; } = false;
    }
}
