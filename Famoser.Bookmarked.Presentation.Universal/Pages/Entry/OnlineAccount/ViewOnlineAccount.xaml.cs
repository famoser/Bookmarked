using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Famoser.Bookmarked.View.ViewModels.Entry;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Famoser.Bookmarked.Presentation.Universal.Pages.Entry.OnlineAccount
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewOnlineAccount : Page
    {
        public ViewOnlineAccount()
        {
            this.InitializeComponent();
        }

        OnlineAccountViewModel OnlineAccountViewModel => DataContext as OnlineAccountViewModel;

        private void HyperlinkUsername_OnClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            if (OnlineAccountViewModel != null && OnlineAccountViewModel.CopyUsernameToClipboard.CanExecute(null))
                OnlineAccountViewModel.CopyPasswordToClipboard.Execute(null);
        }

        private void HyperlinkPassword_OnClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            if (OnlineAccountViewModel != null && OnlineAccountViewModel.CopyPasswordToClipboard.CanExecute(null))
                OnlineAccountViewModel.CopyUsernameToClipboard.Execute(null);
        }
    }
}
