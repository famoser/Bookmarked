namespace Famoser.Bookmarked.Business.Models
{
    public class CsvExportEntry
    {
        public string Folder { get; set; }
        public string ContentType { get; set; }
        public string PrivateNote { get; set; }
        public string WebpageUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string CardholderName { get; set; }
        public string Number { get; set; }
        public string VerificationNumber { get; set; }
        public string Pin { get; set; }
        public string ExpireDate { get; set; }
        public string IssuingBank { get; set; }
        public string IssuingBankNumber { get; set; }
    }
}
