using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.View.Model;
using Famoser.Bookmarked.View.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Famoser.Bookmarked.Presentation.Universal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            this.InitializeComponent();
        }

        private NavigationViewModel ViewModel => DataContext as NavigationViewModel;

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is FolderModel folder)
            {
                if (ViewModel.SelectFolderCommand.CanExecute(folder))
                {
                    ViewModel.SelectFolderCommand.Execute(folder);
                }
            }
            if (e.ClickedItem is EntryModel entry)
            {
                if (ViewModel.SelectEntryCommand.CanExecute(entry))
                {
                    ViewModel.SelectEntryCommand.Execute(entry);
                }
            }
            if (e.ClickedItem is ContentTypeModel contentType)
            {
                if (ViewModel.AddContentTypeCommand.CanExecute(contentType))
                {
                    ViewModel.AddContentTypeCommand.Execute(contentType);
                }
            }
        }

        private const string ItemDragData = "ItemDragData";

        private void ListView_OnDragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            if (e.Items.Count > 0)
            {
                // Set the content of the DataPackage
                e.Data.Properties.Add(ItemDragData, e.Items[0]);
                e.Data.RequestedOperation = DataPackageOperation.Move;

                if (ViewModel.SelectedFolder.ParentIds.Count > 0)
                    MoveUpGrid.Visibility = Visibility.Visible;
            }
        }

        private void ListView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            MoveUpGrid.Visibility = Visibility.Collapsed;
        }

        private bool IsValidDropData(DragEventArgs e, StackPanel target = null)
        {
            if (e.DataView?.Properties != null)
            {
                var entryKeyValue = e.DataView.Properties.FirstOrDefault(x => x.Key == ItemDragData && x.Value.GetType() == typeof(EntryModel));
                if (!entryKeyValue.Equals(default(KeyValuePair<string, object>)))
                    return true;

                var folderKeyValue = e.DataView.Properties.FirstOrDefault(x => x.Key == ItemDragData && x.Value.GetType() == typeof(FolderModel));
                if (!folderKeyValue.Equals(default(KeyValuePair<string, object>)))
                {
                    if (target != null)
                    {
                        var folder = folderKeyValue.Value as FolderModel;
                        return folder != (target.DataContext as FolderModel);
                    }
                    return true;
                }
            }
            return false;
        }

        private void Folder_OnDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = IsValidDropData(e, sender as StackPanel) ? DataPackageOperation.Move : DataPackageOperation.None;
        }

        private async void Folder_OnDrop(object sender, DragEventArgs e)
        {
            if (IsValidDropData(e, sender as StackPanel))
            {
                try
                {
                    var def = e.GetDeferral();

                    var item = e.Data.Properties.FirstOrDefault(x => x.Key == ItemDragData);
                    var entryModel = item.Value as EntryModel;
                    var folderModel = item.Value as FolderModel;
                    if (entryModel != null || folderModel != null)
                    {
                        var stackPanel = sender as StackPanel;
                        var folder = stackPanel?.DataContext as FolderModel;

                        if (folder != null)
                        {
                            if (folderModel != null)
                            {
                                if (folder != folderModel)
                                {
                                    await ViewModel.MoveToNewFolderAsync(folderModel, folder);
                                }
                            }
                            else
                            {
                                await ViewModel.MoveToNewFolderAsync(entryModel, folder);
                            }
                        }
                    }

                    def.Complete();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                e.AcceptedOperation = DataPackageOperation.None;
            }
        }

        private void MoveUpGrid_OnDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = IsValidDropData(e) ? DataPackageOperation.Move : DataPackageOperation.None;
        }

        private async void MoveUpGrid_OnDrop(object sender, DragEventArgs e)
        {
            if (IsValidDropData(e))
            {
                try
                {
                    var def = e.GetDeferral();

                    var item = e.Data.Properties.FirstOrDefault(x => x.Key == ItemDragData);
                    var entry = item.Value as EntryModel;
                    var folder = item.Value as FolderModel;

                    if (entry != null)
                    {
                        await ViewModel.MoveOneFolderUpAsync(entry);
                    }
                    else if (folder != null)
                    {
                        await ViewModel.MoveOneFolderUpAsync(folder);
                    }

                    def.Complete();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                e.AcceptedOperation = DataPackageOperation.None;
            }
        }

        private void NavigationPage_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.KeyDown -= CoreWindowOnKeyDown;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.KeyDown += CoreWindowOnKeyDown;
        }

        private void CoreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Escape)
            {
                //deactivate Search & Garbage before navigating back
                if (ViewModel.InSearchMode)
                {
                    ViewModel.InSearchMode = false;
                }
                else if (ViewModel.InGarbageMode)
                {
                    ViewModel.InGarbageMode = false;
                }
                else
                {
                    ViewModel.GoBackCommand.Execute(null);
                }
                return;
            }

            //if in SearchBox prevent KeyBindings
            if (ViewModel.InSearchMode)
            {
                //SearchBox.FocusState foes not work as expected
                //as FocusState does not represent the active state of the control
                //it still returns Unfocused even if this event has been invoked while writing into the SearchBox
                return;
            }

            if (args.VirtualKey == VirtualKey.Back)
            {
                ViewModel.GoBackCommand.Execute(null);
            }
            else if (args.VirtualKey == VirtualKey.F)
            {
                FolderListView.Focus(FocusState.Programmatic);
            }
            else if (args.VirtualKey == VirtualKey.E)
            {
                EntryListView.Focus(FocusState.Programmatic);
            }
            else if (args.VirtualKey == VirtualKey.S)
            {
                ViewModel.InSearchMode = !ViewModel.InSearchMode;
            }
            else if (args.VirtualKey == VirtualKey.G)
            {
                ViewModel.InGarbageMode = !ViewModel.InGarbageMode;
            }
        }

        private void SearchBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var sb = sender as SearchBox;
            if (sb?.IsEnabled == true)
                sb.Focus(FocusState.Programmatic);
        }
    }
}
