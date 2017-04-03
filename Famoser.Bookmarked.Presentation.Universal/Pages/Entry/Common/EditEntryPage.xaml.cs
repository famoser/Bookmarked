using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Famoser.Bookmarked.Presentation.Universal.Entity;
using GalaSoft.MvvmLight.Ioc;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Famoser.Bookmarked.Presentation.Universal.Pages.Entry.Webpage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditEntryPage : Page
    {
        public EditEntryPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is NavigationParameter pm)
            {
                DataContext = SimpleIoc.Default.GetInstance(pm.ViewModelType);
                Title.Text = "Edit " + pm.Name;
                EntryFrame.Navigate(pm.EditFrameType);
            }
        }
    }
}
