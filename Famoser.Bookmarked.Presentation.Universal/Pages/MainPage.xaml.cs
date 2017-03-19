using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Famoser.LectureSync.Business.Models;
using Famoser.LectureSync.View.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Famoser.LectureSync.Presentation.Universal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // If we have a phone contract, hide the status bar
            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.HideAsync();
            }
        }

        private MainViewModel ViewModel => DataContext as MainViewModel;

        private static bool _firstTime = true;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_firstTime)
                return;

            _firstTime = false;
            
            
            if (ViewModel.RefreshCommand.CanExecute(null))
                ViewModel.RefreshCommand.Execute(null);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var lecture = e.ClickedItem as Lecture;
            if (lecture != null)
            {
                if (ViewModel.SelectCourseCommand.CanExecute(lecture.Course))
                {
                    ViewModel.SelectCourseCommand.Execute(lecture.Course);
                }
            }
            var course = e.ClickedItem as Course;
            if (course != null)
            {
                if (ViewModel.SelectCourseCommand.CanExecute(course))
                {
                    ViewModel.SelectCourseCommand.Execute(course);
                }
            }
        }

        private void UIElement_OnTapped(object sender = null, TappedRoutedEventArgs e = null)
        {
            LocationSplitView.IsPaneOpen = !LocationSplitView.IsPaneOpen;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            UIElement_OnTapped();
            WeekDayGrid.Visibility = Visibility.Visible;
            CoursesGrid.Visibility = Visibility.Collapsed;
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UIElement_OnTapped();
            WeekDayGrid.Visibility = Visibility.Collapsed;
            CoursesGrid.Visibility = Visibility.Visible;
        }
    }
}
