using Windows.UI.Xaml.Controls;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.View.ViewModels;
using GalaSoft.MvvmLight.Ioc;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Famoser.Bookmarked.Presentation.Universal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FindPage : Page
    {
        public FindPage()
        {
            this.InitializeComponent();
        }

        private FindViewModel ViewModel => DataContext as FindViewModel;
        private NavigationViewModel NavigationViewModel => SimpleIoc.Default.GetInstance<NavigationViewModel>();

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is EntryModel entry)
            {
                if (NavigationViewModel.SelectEntryCommand.CanExecute(entry))
                {
                    NavigationViewModel.SelectEntryCommand.Execute(entry);
                }
            }
        }
    }
}
