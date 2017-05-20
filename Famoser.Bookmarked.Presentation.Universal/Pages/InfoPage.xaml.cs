using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Famoser.Bookmarked.View.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Famoser.Bookmarked.Presentation.Universal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InfoPage : Page
    {
        public InfoPage()
        {
            this.InitializeComponent();
        }

        private ImportViewModel ImportViewModel => DataContext as ImportViewModel;

        private async void ImportClick(object sender, RoutedEventArgs e)
        {
            var picker =
                new Windows.Storage.Pickers.FileOpenPicker
                {
                    ViewMode = Windows.Storage.Pickers.PickerViewMode.List,
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
                };

            picker.FileTypeFilter.Add(".bmk");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var str = await FileIO.ReadTextAsync(file);
                await ImportViewModel.Import(str);
            }
        }

        private async void ExportClick(object sender, RoutedEventArgs e)
        {
            var picker =
                new Windows.Storage.Pickers.FileSavePicker()
                {
                    DefaultFileExtension = ".bmk",
                    SuggestedFileName = "bookmarked",
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
                };
            
            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                var content = await ImportViewModel.Export();
                if (!string.IsNullOrEmpty(content))
                    await FileIO.WriteTextAsync(file, content);
            }
        }
    }
}
