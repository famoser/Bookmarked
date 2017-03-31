using System.Threading.Tasks;

namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IPasswordService
    {
        string GetPasswordAsync();
        Task<bool> SetPassword(string password);
        Task<bool> TryPassword(string password);
    }
}
