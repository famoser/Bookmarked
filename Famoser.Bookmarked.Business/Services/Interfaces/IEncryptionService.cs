namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IEncryptionService
    {
        string Decrypt(string content, string password);
        string Encrypt(string content, string password);

        string EncryptRaw(string content, byte[] password);
        string DecryptRaw(string content, byte[] password);
    }
}
