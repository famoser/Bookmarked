using System.Threading.Tasks;

namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IPasswordService
    {
        string GetPassword();
        Task<bool> SetPasswordAsync(string password);
        Task<bool> TryPasswordAsync(string password);
        Task<bool> CheckIsFirstTimeAsync();
    }
}
