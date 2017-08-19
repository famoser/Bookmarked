using System;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Services.Interfaces;

namespace Famoser.Bookmarked.View.Services.Interfaces
{
    public interface IInteractionService
    {
        void OpenInBrowser(Uri uri);
        Task<bool> ConfirmMessageAsync(string message);
        Task ShowMessageAsync(string message);
        void CopyToClipboard(string message);
        void CloseApplication();
        Task<bool> ClearCacheAsync();
        
        Task<bool> SaveFileAsync(string content, string extension, string defaultFileName);
        Task<string> GetFileContentAsync(string extension);
    }
}
