using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace INTEGRA_7_Xamarin
{
    public partial class UIHandler
    {
        public enum FavoriteAction
        {
            SHOW,
            ADD,
            REMOVE
        }

        public FavoriteAction favoriteAction = FavoriteAction.SHOW;

        Grid Favorites_grLeftColumn = null;
        Button Favorites_lblGroups = null;
        ListView Favorites_lvGroupList = null;
        public ObservableCollection<String> Favorites_ocGroupList = null;
        Grid Favorites_grMiddleColumn = null;
        Button Favorites_lblTones = null;
        ListView Favorites_lvToneList = null;
        public ObservableCollection<String> Favorites_ocToneList = null;
        Editor Favorites_edNewFolderName = null;
        Button Favorites_btnAddFolder = null;
        Button Favorites_btnDeleteFolder = null;
        ListView Favorites_lvHelp1 = null;
        ListView Favorites_lvHelp2 = null;
        Button Favorites_btnContext = null;
        Button Favorites_btnPlay = null;
        Button Favorites_btnBackup = null;
        Button Favorites_btnRestore = null;
        Button Favorites_btnReturn = null;
        Grid Favorites_grRightColumn = null;

        public void DrawFavoritesPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Favorites
            // --------------------------------------------------------------------------------------------
            // | Grouplist          | Tonelist                                     | Foldername editor    |
            // |--------------------|----------------------------------------------|----------------------|
            // |                    |                                              | Add folder button    |
            // |                    |                                              |----------------------|
            // |                    |                                              | Delete folder button |
            // |                    |                                              |----------------------|
            // |                    |                                              | Help text area       |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |                      |
            // |                    |                                              |----------------------|
            // |                    |                                              | Context button       |
            // |                    |                                              |----------------------|
            // |                    |                                              | Play button          |
            // |                    |                                              |----------------------|
            // |                    |                                              | Backup buttton       |
            // |                    |                                              |----------------------|
            // |                    |                                              | Restore button       |
            // |                    |                                              |----------------------|
            // |                    |                                              | Return button        |
            // --------------------------------------------------------------------------------------------

            // Create all controls ------------------------------------------------------------------------
            Favorites_lblGroups = new Button();
            Favorites_lblGroups.Text = "Groups";
            Favorites_lblGroups.IsEnabled = false;

            Favorites_lvGroupList = new ListView();
            Favorites_ocGroupList = new ObservableCollection<String>();
            Favorites_lvGroupList.ItemsSource = Favorites_ocGroupList;

            Favorites_lblTones = new Button();
            Favorites_lblTones.Text = "Tones";
            Favorites_lblTones.IsEnabled = false;

            Favorites_lvToneList = new ListView();
            Favorites_ocToneList = new ObservableCollection<String>();
            Favorites_lvToneList.ItemsSource = Favorites_ocToneList;

            Favorites_edNewFolderName = new Editor();

            Favorites_btnAddFolder = new Button();
            Favorites_btnAddFolder.Text = "Add folder";

            Favorites_btnDeleteFolder = new Button();
            Favorites_btnDeleteFolder.Text = "Delete selected folder";

            Favorites_lvHelp1 = new ListView();

            Favorites_lvHelp2 = new ListView();

            Favorites_btnContext = new Button();
            Favorites_btnContext.Text = "";

            Favorites_btnPlay = new Button();
            Favorites_btnPlay.Text = "Play";

            Favorites_btnBackup = new Button();
            Favorites_btnBackup.Text = "Backup to file";

            Favorites_btnRestore = new Button();
            Favorites_btnRestore.Text = "Restore from file";

            Favorites_btnReturn = new Button();
            Favorites_btnReturn.Text = "Return";

            // Add handlers -------------------------------------------------------------------------------

            Favorites_lvGroupList.ItemSelected += Favorites_lvGroupList_ItemSelected;
            Favorites_lvToneList.ItemSelected += Favorites_lvToneList_ItemSelected;
            Favorites_edNewFolderName.TextChanged += Favorites_edNewFolderName_TextChanged;
            Favorites_btnAddFolder.Clicked += Favorites_btnAddFolder_Clicked;
            Favorites_btnDeleteFolder.Clicked += Favorites_btnDeleteFolder_Clicked;
            Favorites_btnContext.Clicked += Favorites_btnContext_Clicked;
            Favorites_btnPlay.Clicked += Favorites_btnPlay_Clicked;
            Favorites_btnBackup.Clicked += Favorites_btnBackup_Clicked;
            Favorites_btnRestore.Clicked += Favorites_btnRestore_Clicked;
            Favorites_btnReturn.Clicked += Favorites_btnReturn_Clicked;

            // Assemble grids with controls ---------------------------------------------------------------

            // A grid for the left column:
            Favorites_grLeftColumn = new Grid();
            Favorites_grLeftColumn.Children.Add(new GridRow(0, new View[] { Favorites_lblGroups }, null, false, false).Row);
            Favorites_grLeftColumn.Children.Add(new GridRow(1, new View[] { Favorites_lvGroupList }, null, false, false).Row);

            RowDefinitionCollection Favorites_rdcLeft = new RowDefinitionCollection();
            Favorites_rdcLeft.Add(new RowDefinition());
            Favorites_rdcLeft.Add(new RowDefinition());
            Favorites_rdcLeft[0].Height = new GridLength(headingHeight, GridUnitType.Absolute);
            Favorites_rdcLeft[1].Height = new GridLength(0, GridUnitType.Auto);

            Favorites_grLeftColumn.RowDefinitions.Add(Favorites_rdcLeft[0]);
            Favorites_grLeftColumn.RowDefinitions.Add(Favorites_rdcLeft[1]);

            // A grid for the middle column:
            Favorites_grMiddleColumn = new Grid();
            Favorites_grMiddleColumn.Children.Add(new GridRow(0, new View[] { Favorites_lblTones }, null, false, false).Row);
            Favorites_grMiddleColumn.Children.Add(new GridRow(1, new View[] { Favorites_lvToneList }, null, false, false).Row);

            RowDefinitionCollection Favorites_rdcMiddle = new RowDefinitionCollection();
            Favorites_rdcMiddle.Add(new RowDefinition());
            Favorites_rdcMiddle.Add(new RowDefinition());
            Favorites_rdcMiddle[0].Height = new GridLength(headingHeight, GridUnitType.Absolute);
            Favorites_rdcMiddle[1].Height = new GridLength(0, GridUnitType.Auto);

            Favorites_grMiddleColumn.RowDefinitions.Add(Favorites_rdcMiddle[0]);
            Favorites_grMiddleColumn.RowDefinitions.Add(Favorites_rdcMiddle[1]);

            // A grid for the right column:
            Favorites_grRightColumn = new Grid();
            Favorites_grRightColumn.Children.Add(new GridRow(0, new View[] { Favorites_edNewFolderName }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(1, new View[] { Favorites_btnAddFolder }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(2, new View[] { Favorites_btnDeleteFolder }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(3, new View[] { Favorites_lvHelp1 }, null, false, false).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(4, new View[] { Favorites_lvHelp2 }, null, false, false).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(5, new View[] { Favorites_btnContext }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(6, new View[] { Favorites_btnPlay }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(7, new View[] { Favorites_btnBackup }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(8, new View[] { Favorites_btnRestore }).Row);
            Favorites_grRightColumn.Children.Add(new GridRow(9, new View[] { Favorites_btnReturn }).Row);

            RowDefinitionCollection Favorites_rdcRight = new RowDefinitionCollection();

            for (Int32 i = 0; i < 10; i++)
            {
                Favorites_rdcRight.Add(new RowDefinition());
                if (i == 3 || i == 4)
                {
                    Favorites_rdcRight[i].Height = new GridLength(5, GridUnitType.Star);
                }
                else
                {
                    Favorites_rdcRight[i].Height = new GridLength(1, GridUnitType.Star);
                }
                Favorites_grRightColumn.RowDefinitions.Add(Favorites_rdcRight[i]);
            }

            // Assemble FavoritesStackLayout --------------------------------------------------------------

            FavoritesStackLayout = new StackLayout();
            FavoritesStackLayout.Children.Add((new GridRow(0, new View[] { Favorites_grLeftColumn, Favorites_grMiddleColumn, Favorites_grRightColumn })).Row);
            FavoritesStackLayout.BackgroundColor = Color.Black;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Handlers
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Favorites_btnReturn_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowLibrarianPage();
        }

        private void Favorites_btnRestore_Clicked(object sender, EventArgs e)
        {
            myFileIO.LoadFavorites();
        }

        private void Favorites_btnBackup_Clicked(object sender, EventArgs e)
        {
            t.Trace("private void btnSave_Click (" + "object" + sender + ", " + "RoutedEventArgs" + e + ", " + ")");
            //Save();
        }

        private void Favorites_btnPlay_Clicked(object sender, EventArgs e)
        {
            commonState.player.Play();
        }

        private void Favorites_btnContext_Clicked(object sender, EventArgs e)
        {
            //if (commonState.currentTone != null && lvFolders.SelectedIndex > -1)
            //{
            //    t.Trace("private void btnContext_Click (" + "object" + sender + ", " + "RoutedEventArgs" + e + ", " + ")");
            //    ListViewItem item = (ListViewItem)lvFolders.ContainerFromItem(lvFolders.Items[lvFolders.SelectedIndex]);
            //    if (commonState.command == "Add" && ((String)item.Content).StartsWith("*"))
            //    {
            //        commonState.favoritesList.folders[lvFolders.SelectedIndex].FavoritesTones.Add(commonState.currentTone);
            //        item.Content = ((String)item.Content).TrimStart('*');
            //        UpdateFavoritesList(lvFolders.SelectedIndex);
            //        SaveToLocalSettings();
            //    }
            //    else if (commonState.command == "Delete")
            //    {
            //        DeleteFavorite(commonState.currentTone);
            //    }
            //    else if (commonState.command == "Show")
            //    {
            //        //FindFavoriteByNameAndFolder()
            //        commonState.currentTone = (Tone)lvFavorites.SelectedItem;
            //        this.Frame.Navigate(typeof(MainPage), commonState);
            //    }
            //}
        }

        private void Favorites_btnDeleteFolder_Clicked(object sender, EventArgs e)
        {
            //MessageDialog warning = new MessageDialog("Warning: Deleting a folder will also delete all containing favorites.\r\n\r\n" +
            //    "Are you sure you want to do this?");
            //warning.Title = "Warning!";
            //warning.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
            //warning.Commands.Add(new UICommand { Label = "No", Id = 1 });
            //var response = await warning.ShowAsync();
            //if ((Int32)response.Id == 0)
            //{
            //    commonState.favoritesList.folders.RemoveAt(lvFolders.SelectedIndex);
            //    SaveToLocalSettings();
            //    UpdateFoldersList();
            //}
        }

        private void Favorites_btnAddFolder_Clicked(object sender, EventArgs e)
        {
            //t.Trace("private void btnNewFolder_Click (" + "object" + sender + ", " + "RoutedEventArgs" + e + ", " + ")");
            //commonState.favoritesList.folders.Add(new Integra_7_uwp.FavoritesFolder(tbNewFolder.Text.Trim()));
            //tbNewFolder.Text = "";
            //UpdateFoldersList();
        }

        private void Favorites_edNewFolderName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //t.Trace("private void tbNewFolder_KeyUp (" + "object" + sender + ", " + "KeyRoutedEventArgs" + e + ", " + ")");
            //if (!String.IsNullOrEmpty(tbNewFolder.Text))
            //{
            //    Boolean found = false;
            //    foreach (FavoritesFolder folder in commonState.favoritesList.folders)
            //    {
            //        if (folder.Name == tbNewFolder.Text.Trim())
            //        {
            //            found = true;
            //            break;
            //        }
            //    }
            //    if (!found)
            //    {
            //        btnNewFolder.IsEnabled = true;
            //        if (e.Key == VirtualKey.Enter)
            //        {
            //            btnNewFolder_Click(new object(), new RoutedEventArgs());
            //        }
            //    }
            //    else
            //    {
            //        btnNewFolder.IsEnabled = false;
            //    }
            //}
            //else
            //{
            //    btnNewFolder.IsEnabled = false;
            //}
        }

        private void Favorites_lvToneList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //if (handleControlEvents)
            //{
            //    t.Trace("private void lvFavorites_SelectionChanged(" + ((ListView)sender).SelectedIndex.ToString() + ")");
            //    try
            //    {
            //        // List only contains the name, lookup the currentTone by name and folder:
            //        commonState.currentTone = (Tone)lvFavorites.Items[lvFavorites.SelectedIndex];
            //        commonState.currentTone.Index = commonState.toneList.Get(commonState.currentTone);
            //    }
            //    catch { }
            //    // The user came here to browse favorites, and has now selected one.
            //    // The favorite should be selectable via the context button.
            //    if (commonState.command == "Show")
            //    {
            //        btnContext.Content = "Select " + commonState.currentTone.Name;
            //        btnContext.Visibility = Visibility.Visible;
            //        btnContext.IsEnabled = true;
            //        if (commonState.currentTone.Index > -1)
            //        {
            //            List<String> tone = commonState.toneList.Tones[commonState.currentTone.Index];
            //            commonState.midi.ProgramChange(commonState.midi.MidiOutPortChannel, tone[4], tone[5], tone[7]);
            //        }
            //        else
            //        {
            //            Int32 index = commonState.toneList.Get(commonState.currentTone);
            //            if (index > -1)
            //            {
            //                List<String> tone = commonState.toneList.Tones[commonState.toneList.Get(commonState.currentTone)];
            //                commonState.midi.ProgramChange(commonState.midi.MidiOutPortChannel, tone[4], tone[5], tone[7]);
            //            }
            //        }
            //    }
            //    // The user came here to add a favorite, and now selects another favorite.
            //    // The favorite should be allowed to be stored in another folder.
            //    else if (commonState.command == "Add")
            //    {
            //        btnContext.Content = "Copy " + commonState.currentTone.Name;
            //        btnContext.Visibility = Visibility.Visible;
            //        btnContext.IsEnabled = true;
            //        tbContextInstructions.Text = "You have selected a favorite and it is now possible to copy it to another " +
            //            "folder. Select the destination folder and clic the \'Copy " + commonState.currentTone.Name + " button.";
            //        //UpdateFoldersList();
            //    }
            //    // The user came here to delete a favorite, and now selects another favorite.
            //    // The favorite should be allowed to be deleted.
            //    else if (commonState.command == "Delete")
            //    {
            //        btnContext.Content = "Delete " + commonState.currentTone.Name;
            //        btnContext.Visibility = Visibility.Visible;
            //        btnContext.IsEnabled = true;
            //        UpdateFoldersList();
            //    }
            //}
        }

        private void Favorites_lvGroupList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //t.Trace("private void lvFolders_SelectionChanged(" + ((ListView)sender).SelectedIndex.ToString() + ")");
            //UpdateFavoritesList(lvFolders.SelectedIndex);
            //currentFolder = lvFolders.SelectedIndex;
            //if (commonState.command == "Add" || commonState.command == "Delete")
            //{
            //    if (lvFolders.SelectedItem.ToString().StartsWith("*"))
            //    {
            //        btnContext.IsEnabled = true;
            //    }
            //    else
            //    {
            //        btnContext.IsEnabled = false;
            //    }
            //}
            //if (lvFolders.SelectedIndex > -1)
            //{
            //    btnDeleteFolder.IsEnabled = true;
            //}
        }

        private void Favorites_NotYetImplemented_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowLibrarianPage();
        }

        public void ShowFavoritesPage(FavoriteAction favoriteAction)
        {
            if (FavoritesStackLayout == null)
            {
                DrawFavoritesPage();
            }
            mainStackLayout.Children.Add(FavoritesStackLayout);
        }

        /// <summary>
        /// Right tapping or double-clicking a favorite should take immediate action
        /// without the need to click the context button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void lvFavorites_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        //{
        //    if (handleControlEvents)
        //    {
        //        t.Trace("private void lvFavorites_DoubleTapped(" + ((ListView)sender).SelectedIndex.ToString() + ")");
        //        try
        //        {
        //            // List only contains the name, lookup the currentTone by name and folder:
        //            commonState.currentTone = (Tone)lvFavorites.Items[lvFavorites.SelectedIndex];
        //            commonState.currentTone.Index = commonState.toneList.Get(commonState.currentTone);
        //        }
        //        catch
        //        {
        //            commonState.currentTone.Index = -1;
        //        }
        //        // If the user came here to pick a favorite, it is now selected.
        //        // Just return to the main page:
        //        if (commonState.command == "Show")
        //        {
        //            this.Frame.Navigate(typeof(MainPage), commonState);
        //        }
        //        // If the user came here to add a favorite, selecting one would be to get it.
        //        // Just get it and return to the main page:
        //        else if (commonState.command == "Add")
        //        {
        //            if (commonState.currentTone.Index < 0)
        //            {
        //                // List only contains the name, lookup the currentTone by name and folder:
        //                commonState.currentTone = FindFavoriteByNameAndFolder((String)lvFavorites.SelectedItem, ((String)lvFolders.SelectedItem).TrimStart('*'));
        //            }
        //            this.Frame.Navigate(typeof(MainPage), commonState);
        //        }
        //        // If the user came here to delete a favorite, selecting another would be for
        //        // deleting that one too (because of right tap or double click:
        //        else if (commonState.command == "Delete")
        //        {
        //            // List only contains the name, lookup the currentTone by name and folder:
        //            Tone tone = FindFavoriteByNameAndFolder((String)lvFavorites.SelectedItem, ((String)lvFolders.SelectedItem).TrimStart('*'));
        //            DeleteFavorite(tone);
        //            UpdateFoldersList();
        //        }
        //    }
        //}

        //private void lvFolders_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        //{
        //    if (commonState.currentTone != null && lvFolders.SelectedIndex > -1)
        //    {
        //        t.Trace("private void lvFolders_DoubleTapped(" + ((ListView)sender).SelectedIndex.ToString() + ")");
        //        ListViewItem item = (ListViewItem)lvFolders.ContainerFromItem(lvFolders.Items[lvFolders.SelectedIndex]);
        //        if (commonState.command == "Add" && ((String)item.Content).StartsWith("*"))
        //        {
        //            commonState.favoritesList.folders[lvFolders.SelectedIndex].FavoritesTones.Add(commonState.currentTone);
        //            item.Content = ((String)item.Content).TrimStart('*');
        //            UpdateFavoritesList(lvFolders.SelectedIndex);
        //            SaveToLocalSettings();
        //        }
        //        if (commonState.command == "Delete" && ((String)item.Content).StartsWith("*"))
        //        {
        //            Tone tone = commonState.favoritesList.folders[lvFolders.SelectedIndex].ByToneName(commonState.currentTone.Name);
        //            commonState.favoritesList.folders[lvFolders.SelectedIndex].FavoritesTones.Remove(tone);
        //            item.Content = ((String)item.Content).TrimStart('*');
        //            UpdateFavoritesList(lvFolders.SelectedIndex);
        //            SaveToLocalSettings();
        //        }
        //    }
        //}

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Public functions to be called from device dependent code
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Favorites_Restore(String linesToUnpack)
        {
            if (linesToUnpack != "Error" && linesToUnpack != "Empty")
            {
                UnpackFoldersWithFavoritesString(linesToUnpack);
                UpdateFoldersList();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Loca helpers
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    try
        //    {
        //        commonState = (CommonState)e.Parameter;
        //        if (commonState.favoritesList == null)
        //        {
        //            commonState.favoritesList = new FavoritesList();
        //            commonState.favoritesList.folders = new List<FavoritesFolder>();
        //            try
        //            {
        //                if (localSettings.Values.ContainsKey("Favorites"))
        //                {
        //                    String foldersWithFavorites = ((String)localSettings.Values["Favorites"]);
        //                    UnpackFoldersWithFavoritesString(foldersWithFavorites);
        //                }
        //            }
        //            catch
        //            {
        //                localSettings.Values.Remove("Favorites");
        //            }
        //        }
        //        commonState.player.btnPlayStop = btnPlayStop;
        //        if (commonState.player.Playing)
        //        {
        //            btnPlayStop.Content = "Stop";
        //        }
        //        if (commonState.command == "Show")
        //        {
        //            UpdateFoldersList();
        //            if (lvFolders.Items.Count() > 0)
        //            {
        //                lvFolders.SelectedIndex = 0;
        //                UpdateFavoritesList(lvFolders.SelectedIndex);
        //            }
        //        }
        //        if (commonState.command == "Add")
        //        {
        //            tbContextInstructions.Text = "The folder(s) that does not already contain the Tone " + commonState.currentTone.Name +
        //                " has been marked with a \'*\'. Doubletap the folders you wish to add the Tone \'" +
        //                commonState.currentTone.Name + "\' to (or select the folder name and click 'Add " +
        //                commonState.currentTone.Name + "').";
        //            btnContext.Content = "Add " + commonState.currentTone.Name;
        //            btnContext.IsEnabled = false;
        //            UpdateFoldersList();
        //        }
        //        else if (commonState.command == "Delete")
        //        {
        //            tbContextInstructions.Text = "The folder(s) that contains the Tone " + commonState.currentTone.Name +
        //                " has been marked with a \'*\'. Doubletap the folders you wish to remove the Tone \'" +
        //                commonState.currentTone.Name + "\' from (or select the folder name and click 'Delete " +
        //                commonState.currentTone.Name + "').";
        //            btnContext.Content = "Delete " + commonState.currentTone.Name;
        //            btnContext.IsEnabled = false;
        //            UpdateFoldersList();
        //        }
        //        else if (commonState.command == "Show")
        //        {
        //            tbContextInstructions.Text = "Please select a folder and double-click a favorite to select it.\r\n\r\n" +
        //                "You can also click it and then click the \'Select\'button.\r\n\\r\n." +
        //                "Press play if you want to hear a sample of the selected tone before you go back to the librarian.";
        //        }
        //        commonState.player = new Player(commonState, ref btnPlayStop);
        //    }
        //    catch { }
        //}

        private void SelectFolder(String folderName)
        {
            t.Trace("private void SelectFolder (" + "String" + folderName + ", " + ")");
            try
            {
                foreach (String item in Favorites_ocGroupList.AsQueryable())
                {
                    if (item.TrimStart('*') == folderName)
                    {
                        Favorites_lvGroupList.SelectedItem = item;
                        return;
                    }
                }
            }
            catch (Exception e) { }
        }

        private void UnpackFoldersWithFavoritesString(String foldersWithFavorites)
        {
            t.Trace("private void UnpackFoldersWithFavoritesString (" + "String" + foldersWithFavorites + ", " + ")");
            // Format: [Folder name\v[Group index\tCategory index\tTone index\tGroup\tCategory\tTone\b]\f...]...
            // I.e. Split all by \f to get all folders with content.
            // Split each folder by \v to get folder name and all favorites together.
            // Split favorites by \b to get all favorites one by one.
            // Split each favorite by \t to get the 6 parts (3 indexes, 3 names).
            FavoritesFolder folder = null;
            commonState.favoritesList.folders.Clear();
            foreach (String foldersWithFavoritePart in foldersWithFavorites.Split('\f'))
            {
                String[] folderWithFavorites = foldersWithFavoritePart.Split('\v');
                // Folder name:
                folder = new FavoritesFolder(folderWithFavorites[0]);
                commonState.favoritesList.folders.Add(folder);
                if (folderWithFavorites.Length > 1)
                {
                    String[] favorites = folderWithFavorites[1].Split('\b');
                    foreach (String favorite in favorites)
                    {
                        String[] favoriteParts = favorite.Split('\t');
                        try
                        {
                            if (favoriteParts.Length == 6)
                            {
                                folder.FavoritesTones.Add(new Tone(Int32.Parse(favoriteParts[0]), Int32.Parse(favoriteParts[1]),
                                    Int32.Parse(favoriteParts[2]), favoriteParts[3], favoriteParts[4], favoriteParts[5]));
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        private void UpdateFoldersList(Int32 SelectedIndex = -1)
        {
            if (handleControlEvents)
            {
                t.Trace("private void UpdateFoldersList(SelectedIndex = " + SelectedIndex.ToString() + ")");
                handleControlEvents = false;
                try
                {
                    Int32 count = 0;
                    Favorites_ocToneList.Clear(); // Since we will not have a selected folder, do not show favorites!
                    Int32 selectedIndex = -1;
                    Favorites_btnDeleteFolder.IsEnabled = false;
                    Favorites_ocGroupList.Clear();
                    //Favorites_lvGroupList.ItemSelected -= Favorites_lvGroupList_ItemSelected;
                    //Favorites_lvGroupList.DoubleTapped -= Favorites_lvGroupList_DoubleTapped;
                    //Favorites_lvGroupList = new ListView();
                    //Favorites_lvGroupList.Name = "Favorites_lvGroupList";
                    //Favorites_lvGroupList.Margin = new Thickness(2, 2, 2, 2);
                    //Favorites_lvGroupList.BorderThickness = new Thickness(1);
                    //Favorites_lvGroupList.BorderBrush = black;
                    //Favorites_lvGroupList.Visibility = Visibility.Visible;
                    //Favorites_lvGroupList.SelectionChanged += Favorites_lvGroupList_SelectionChanged;
                    //Favorites_lvGroupList.DoubleTapped += Favorites_lvGroupList_DoubleTapped;
                    //Favorites_ocGroupList gcFolders.Children.Add(Favorites_lvGroupList);
                    UInt16 i = 0;
                    foreach (FavoritesFolder folder in commonState.favoritesList.folders)
                    {
                        Boolean mark = false;
                        if (commonState.command == "Add" || commonState.command == "Delete")
                        {
                            foreach (Tone fav in folder.FavoritesTones)
                            {
                                if (fav.Group == commonState.currentTone.Group
                                    && fav.Category == commonState.currentTone.Category
                                    && fav.Name == commonState.currentTone.Name)
                                {
                                    mark = true;
                                    selectedIndex = i;
                                    count++;
                                }
                            }
                        }
                        if (commonState.command == "Add")
                        {
                            mark = !mark;
                        }

                        if (mark)
                        {
                            Favorites_ocGroupList.Add("*" + folder.Name);
                        }
                        else
                        {
                            Favorites_ocGroupList.Add(folder.Name);
                        }
                        i++;
                    }
                    if ((SelectedIndex > -1 && commonState.command == "Show") || (count > 0 && commonState.command == "Delete") || (count == 0 && commonState.command == "Add"))
                    {
                        // There are still items to delete or still room for more items, 
                        // stay in marked mode:
                        //Favorites_lvGroupList.ItemSelected = fa selectedIndex;
                    }
                    else
                    {
                        // There are no more items to delete, or no folders that 
                        // does not have the item to add, go to normal mode:
                        //if (currentFolder > -1 && currentFolder < Favorites_lvGroupList.Items.Count())
                        //{
                        //    Favorites_lvGroupList.SelectedIndex = currentFolder;
                        //}
                        //else
                        //{
                        //    Favorites_lvGroupList.SelectedIndex = 0;
                        //}
                    }
                    handleControlEvents = true;
                    if (Favorites_lvGroupList.SelectedItem == null)
                    {
                        if (Favorites_ocGroupList.Count > 0)
                        {
                            Favorites_lvGroupList.SelectedItem = Favorites_ocGroupList[0];
                        }
                    }
                    UpdateFavoritesList(Favorites_lvGroupList.SelectedItem.ToString());
                    Favorites_lvGroupList.ItemsSource = Favorites_ocGroupList;
                }
                catch { }
                handleControlEvents = true;
            }
        }

        private void UpdateFavoritesList(String folderName)
        {
            //if (handleControlEvents)
            //{
            //    t.Trace("private void UpdateFavoritesList(folderIndex = " + folderIndex.ToString() + ")");
            //    if (folderIndex > -1 && folderIndex < commonState.favoritesList.folders.Count())
            //    {
            // Finde the folder by name:
            FavoritesFolder favoritesFolder = null;
            for (Int32 i = 0; i < commonState.favoritesList.folders.Count() && favoritesFolder == null; i++)
            {
                if (commonState.favoritesList.folders[i].Name == folderName)
                {
                    favoritesFolder = commonState.favoritesList.folders[i];
                }
            }
                    Favorites_ocToneList.Clear();
                    foreach (Tone fav in commonState.favoritesList.folders[commonState.favoritesList.folders.IndexOf(favoritesFolder)].FavoritesTones)
                    {
                        Favorites_ocToneList.Add(fav.Name);
                    }
            //    }
            //}
        }

        private Tone FindFavoriteByNameAndFolder(String Name, String FolderName)
        {
            foreach (FavoritesFolder folder in commonState.favoritesList.folders)
            {
                if (folder.Name == FolderName || ("* " + folder.Name) == FolderName)
                {
                    foreach (Tone favorite in folder.FavoritesTones)
                    {
                        if (favorite.Name == Name)
                        {
                            return favorite;
                        }
                    }
                }
            }
            return null;
        }
    }
}
