using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Famoser.Bookmarked.Presentation.Universal.Entity;
using Famoser.Bookmarked.View.Services.Interfaces;
using Famoser.Bookmarked.View.ViewModels.Entry.Abstract;
using Famoser.Bookmarked.View.ViewModels.Folder;
using GalaSoft.MvvmLight.Ioc;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Famoser.Bookmarked.Presentation.Universal.Pages.Entry.Common
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddEntryPage : Page
    {
        public AddEntryPage()
        {
            this.InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is NavigationParameter pm)
            {
                DataContext = SimpleIoc.Default.GetInstance(pm.ViewModelType);
                EntryFrame.Navigate(pm.EditFrameType);
            }
        }
    }
}
