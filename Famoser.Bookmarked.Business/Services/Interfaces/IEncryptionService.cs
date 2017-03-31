namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IEncryptionService
    {
        string Decrypt(string content, string password);
        string Encrypt(string json, string password);
    }
}
