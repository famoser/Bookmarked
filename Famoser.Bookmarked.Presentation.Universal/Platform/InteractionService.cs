using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Famoser.LectureSync.View.Services.Interfaces;
using GalaSoft.MvvmLight.Threading;

namespace Famoser.LectureSync.Presentation.Universal.Platform
{
    internal class InteractionService : IInteractionService
    {
        public void OpenInBrowser(Uri uri)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Windows.System.Launcher.LaunchUriAsync(uri);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    
        public void CheckBeginInvokeOnUi(Action action)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(action);
        }

        public async Task<bool> ConfirmMessage(string message)
        {
            var dialog = new MessageDialog( message);

            dialog.Commands.Add(new UICommand("Confirm") { Id = 0 });
            dialog.Commands.Add(new UICommand("Abort") { Id = 1 });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            return (int)result.Id == 0;
        }
    }
}
