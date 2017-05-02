using Windows.ApplicationModel.DataTransfer;
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

        OnlineAccountViewModel DataContext => DataContext;

        private void Hyperlink_OnClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            if (DataContext != null && DataContext.CopyPasswordToClipboard.CanExecute(null))
                DataContext.CopyPasswordToClipboard.Execute(null);
        }
    }
}
