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

        Task<string> ImportExportFileAsync();

        Task<string> ImportCredentialsFileAsync();

        Task<bool> SaveExportFileAsync(string content);

        Task<bool> SaveCredentialsFileAsync(string content);

        void CloseApplication();

        Task<bool> ClearCacheAsync();
    }
}
