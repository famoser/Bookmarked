using System.Threading.Tasks;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface ILoginService
    {
        string HashPassword(string input);
        void RegisterValidPassword(string hashedPassword);
        Task<string> TryAlternativeLogin();
    }
}
