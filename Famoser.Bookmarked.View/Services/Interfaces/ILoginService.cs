using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface ILoginService
    {
        string HashPassword(string input);
        void RegisterValidPassword(string hashedPassword);
        Task<string> TryAlternativeLogin();
        void InvalidateAlternativeLogin();
    }
}
