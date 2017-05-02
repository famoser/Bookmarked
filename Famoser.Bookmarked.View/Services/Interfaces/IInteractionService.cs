using System;
using System.Threading.Tasks;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface IInteractionService
    {
        void OpenInBrowser(Uri uri);
        void CheckBeginInvokeOnUi(Action action);
        string HashPassword(string input);

        Task<bool> ConfirmMessage(string message);

        void CopyToClipboard(string message);
    }
}
