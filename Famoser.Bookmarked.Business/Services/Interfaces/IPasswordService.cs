using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Enum;

namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IPasswordService
    {
        string GetPassword();
        Task<bool> SetPasswordAsync(string password);
        Task<bool> TryPasswordAsync(string password);
        Task<bool> CheckIsFirstTimeAsync();
        string GeneratePassword(PasswordType type, int length);
    }
}
