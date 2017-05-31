using System;
using System.Threading.Tasks;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface IInteractionService
    {
        void OpenInBrowser(Uri uri);
        void CheckBeginInvokeOnUi(Action action);
        Task<bool> ConfirmMessageAsync(string message);
        Task ShowMessageAsync(string message);
        void CopyToClipboard(string message);
        void CloseApplication();
        Task<bool> ClearCacheAsync();
    }
}
