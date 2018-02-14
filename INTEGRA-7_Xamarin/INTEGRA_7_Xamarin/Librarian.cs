using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public partial class UIHandler
    {
        enum ToneNamesFilter
        {
            ALL = 0,
            PRESET = 1,
            USER = 2
        }
        ToneNamesFilter toneNamesFilter = ToneNamesFilter.ALL;

        // Librarian controls:
        public Picker Librarian_midiOutputDevice { get; set; }
        public Picker Librarian_midiInputDevice { get; set; }
        public Picker Librarian_midiOutputChannel { get; set; }
        public Picker Librarian_midiInputChannel { get; set; }

        Boolean allowListViewUpdates = true;
        Boolean previousAllowListViewUpdates = true;
        Grid Librarian_gridGroups;
        Button Librarian_lblGroups;
        ListView Librarian_lvGroups;
        ObservableCollection<String> Librarian_Groups;
        Grid Librarian_gridCategories;
        Button Librarian_lblCategories;
        ListView Librarian_lvCategories;
        ObservableCollection<String> Librarian_Categories;
        Grid Librarian_gridTones;
        Button Librarian_filterPresetAndUser;
        ListView Librarian_lvToneNames;
        ObservableCollection<String> Librarian_ToneNames;
        Grid Librarian_gridToneData;
        LabeledTextInput Librarian_tbSearch;
        LabeledText Librarian_ltToneName;
        LabeledText Librarian_ltType;
        LabeledText Librarian_ltToneNumber;
        LabeledText Librarian_ltBankNumber;
        LabeledText Librarian_ltBankMSB;
        LabeledText Librarian_ltBankLSB;
        LabeledText Librarian_ltProgramNumber;
        Button Librarian_btnEditTone;
        Button Librarian_btnEditStudioSet;
        Button Librarian_btnResetVolume;
        Button Librarian_btnMotionalSurround;
        Button Librarian_btnAddFavorite;
        Button Librarian_btnRemoveFavorite;
        Button Librarian_btnPlay;
        Button Librarian_btnFavorites;
        Button Librarian_btnResetHangingNotes;
        Button Librarian_btnPlus12keys;
        Button Librarian_btnMinus12keys;
        MyLabel Librarian_lblKeys;
        Image Librarian_Keyboard;

        public void DrawLibrarianPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Librarian 
            //                                                           ____________________________________ gridTones
            //                                                          /                      ______________ gridToneData
            //                                                         /                      /
            // ____________________________________________________________________________________________
            // | lblGroups            | lblCategories        | filterPresetAndUser |midiOutputDevice/part |
            // |----------------------|----------------------|---------------------|----------------------|
            // | lvGroups             | lvCategories         | lvToneNames         |tbSearch              |
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |ltToneName            |
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |ltType                |
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |ltToneNumber          |
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |ltBankNumber          |
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |ltBankMSB             |
            // |                      |                      |                     |----------------------| Main row 0
            // |                      |                      |                     |ltBankLSB             |
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |ltProgramNumber       |
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |Edit tone| studioset  |
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |Res vol  |Motion.sur. |
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |Favorites|Add  |Remove|
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |Reset hanging  |Play  |  
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |keyrange |+12k | -12k |
            // |------------------------------------------------------------------------------------------|
            // | Keyboard                                                                                 |
            // |                                                                                          | Main row 1
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Create all controls ------------------------------------------------------------------------

            // Make a listview lvGroups for column 0:
            Librarian_lblGroups = new Button();
            Librarian_lblGroups.Text = "Synth types & Expansion slots";
            Librarian_lblGroups.IsEnabled = false;
            Librarian_lvGroups = new ListView();
            Librarian_Groups = new ObservableCollection<String>();
            Librarian_lvGroups.ItemsSource = Librarian_Groups;

            // Make a listview lvCategories for column 1:
            Librarian_lblCategories = new Button();
            Librarian_lblCategories.Text = "Sound categories";
            Librarian_lvCategories = new ListView();
            Librarian_Categories = new ObservableCollection<String>();
            Librarian_lvCategories.ItemsSource = Librarian_Categories;

            // Make Grids for column 0 - 2:
            Librarian_gridGroups = new Grid();
            Librarian_gridCategories = new Grid();
            Librarian_gridTones = new Grid();

            // Make a filter button for column 2:
            Librarian_filterPresetAndUser = new Button();
            Librarian_filterPresetAndUser.Text = "Preset and User";
            Librarian_filterPresetAndUser.BackgroundColor = colorSettings.Background;

            // Make a listview lvToneNames for column 2:
            Librarian_lvToneNames = new ListView();
            Librarian_lvToneNames.BackgroundColor = colorSettings.Background;
            Librarian_Categories = new ObservableCollection<String>();
            //commonState.toneList.Tones = new List<List<String>>();
            Librarian_lvToneNames.ItemsSource = Librarian_ToneNames;
            //Librarian_lvToneNames.Margin = new Thickness(0);

            // Make a Grid for column 3:
            Librarian_gridToneData = new Grid();

            // Make pickers for MIDI:
            Librarian_midiOutputDevice = new Picker();
            Librarian_midiInputDevice = new Picker();
            Librarian_midiOutputChannel = new Picker();
            for (Int32 i = 0; i < 16; i++)
            {
                String temp = "Part " + (i + 1).ToString();
                Librarian_midiOutputChannel.Items.Add(temp);
            }
            Librarian_midiOutputChannel.SelectedIndex = 0;
            Librarian_midiInputChannel = new Picker();
            for (Int32 i = 0; i < 16; i++)
            {
                String temp = "Part " + (i + 1).ToString();
                Librarian_midiInputChannel.Items.Add(temp);
            }
            Librarian_midiInputChannel.SelectedIndex = 0;
            Librarian_midiInputDevice.IsVisible = false;
            Librarian_midiInputChannel.IsVisible = false;

            // Make labeled editor fields:
            Librarian_tbSearch = new LabeledTextInput("Search:", new byte[] { 1, 2 });
            Librarian_ltToneName = new LabeledText("Tone Name:", "Full Grand 1", new byte[] { 1, 2 });
            Librarian_ltType = new LabeledText("Type:", "(Preset)", new byte[] { 1, 2 });
            Librarian_ltToneNumber = new LabeledText("Tone #:", "1", new byte[] { 1, 2 });
            Librarian_ltBankNumber = new LabeledText("Bank #:", "11456", new byte[] { 1, 2 });
            Librarian_ltBankMSB = new LabeledText("Bank MSB:", "89", new byte[] { 1, 2 });
            Librarian_ltBankLSB = new LabeledText("Bank LSB:", "64", new byte[] { 1, 2 });
            Librarian_ltProgramNumber = new LabeledText("Program #:", "1", new byte[] { 1, 2 });

            // Add the keyboard image:
            Librarian_Keyboard = new Image { Aspect = Aspect.Fill };
            Librarian_Keyboard.Source = ImageSource.FromFile("Keyboard.jpg");
            Librarian_Keyboard.HeightRequest = 330;
            Librarian_Keyboard.VerticalOptions = LayoutOptions.StartAndExpand;
            Librarian_Keyboard.HorizontalOptions = LayoutOptions.CenterAndExpand;
            mainStackLayout.Children.Add(Librarian_Keyboard);

            // Add the buttons
            Librarian_btnEditTone = new Button();
            Librarian_btnEditStudioSet = new Button();
            Librarian_btnResetVolume = new Button();
            Librarian_btnMotionalSurround = new Button();
            Librarian_btnAddFavorite = new Button();
            Librarian_btnRemoveFavorite = new Button();
            Librarian_btnPlay = new Button();
            Librarian_btnFavorites = new Button();
            Librarian_btnResetHangingNotes = new Button();
            Librarian_btnPlus12keys = new Button();
            Librarian_btnMinus12keys = new Button();
            Librarian_lblKeys = new MyLabel();

            Librarian_btnEditTone.Text = "Edit tone";
            Librarian_btnEditStudioSet.Text = "Edit studio set";
            Librarian_btnResetVolume.Text = "Reset volume";
            Librarian_btnMotionalSurround.Text = "Motional surround";
            Librarian_btnAddFavorite.Text = "Add";
            Librarian_btnRemoveFavorite.Text = "Remove";
            Librarian_btnPlay.Text = "Play";
            Librarian_btnFavorites.Text = "Favorites";
            Librarian_btnResetHangingNotes.Text = "Reset";
            Librarian_btnPlus12keys.Text = "+12";
            Librarian_btnMinus12keys.Text = "-12";
            Librarian_lblKeys.Text = "Keys 36-96";

            Librarian_btnEditTone.BackgroundColor = colorSettings.Background;
            Librarian_btnEditStudioSet.BackgroundColor = colorSettings.Background;
            Librarian_btnResetVolume.BackgroundColor = colorSettings.Background;
            Librarian_btnMotionalSurround.BackgroundColor = colorSettings.Background;
            Librarian_btnAddFavorite.BackgroundColor = colorSettings.Background;
            Librarian_btnRemoveFavorite.BackgroundColor = colorSettings.Background;
            Librarian_btnPlay.BackgroundColor = colorSettings.Background;
            Librarian_btnFavorites.BackgroundColor = colorSettings.Background;
            Librarian_btnResetHangingNotes.BackgroundColor = colorSettings.Background;
            Librarian_btnPlus12keys.BackgroundColor = colorSettings.Background;
            Librarian_btnMinus12keys.BackgroundColor = colorSettings.Background;
            Librarian_lblKeys.BackgroundColor = colorSettings.Background;

            // Add handlers -------------------------------------------------------------------------------

            Librarian_lvGroups.ItemSelected += Librarian_LvGroups_ItemSelected;
            Librarian_lvCategories.ItemSelected += Librarian_LvCategories_ItemSelected;
            Librarian_filterPresetAndUser.Clicked += Librarian_FilterPresetAndUser_Clicked;
            Librarian_lvToneNames.ItemSelected += Librarian_LvToneNames_ItemSelected;
            Librarian_midiOutputDevice.SelectedIndexChanged += Librarian_MidiOutputDevice_SelectedIndexChanged;
            Librarian_midiOutputChannel.SelectedIndexChanged += Librarian_MidiOutputChannel_SelectedIndexChanged;
            Librarian_tbSearch.Editor.TextChanged += Librarian_Editor_TextChanged;
            Librarian_btnEditTone.Clicked += Librarian_BtnEditTone_Clicked;
            Librarian_btnEditStudioSet.Clicked += Librarian_btnEditStudioSet_Clicked;
            Librarian_btnResetVolume.Clicked += Librarian_btnResetVolume_Clicked;
            Librarian_btnMotionalSurround.Clicked += Librarian_btnMotionalSurround_Clicked;
            Librarian_btnAddFavorite.Clicked += Librarian_btnAddFavorite_Clicked;
            Librarian_btnRemoveFavorite.Clicked += Librarian_btnRemoveFavorite_Clicked;
            Librarian_btnPlay.Clicked += Librarian_btnPlay_Clicked;
            Librarian_btnFavorites.Clicked += Librarian_btnFavorites_Clicked;
            Librarian_btnResetHangingNotes.Clicked += Librarian_btnResetHangingNotes_Clicked;
            Librarian_btnPlus12keys.Clicked += Librarian_btnPlus12keys_Clicked;
            Librarian_btnMinus12keys.Clicked += Librarian_btnMinus12keys_Clicked;

            // Assemble grids with controls ---------------------------------------------------------------

            // Assemble column 0:
            Librarian_gridGroups.Children.Add((new GridRow(0, new View[] { Librarian_lblGroups }, null, false, false)).Row);
            Librarian_gridGroups.Children.Add((new GridRow(1, new View[] { Librarian_lvGroups }, null, false, false)).Row);
            Librarian_gridGroups.RowDefinitions = new RowDefinitionCollection();
            Librarian_gridGroups.RowDefinitions.Add(new RowDefinition());
            Librarian_gridGroups.RowDefinitions.Add(new RowDefinition());
            Librarian_gridGroups.RowDefinitions[0].Height = new GridLength(30, GridUnitType.Absolute);
            Librarian_gridGroups.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);

            // Assemble column 1:
            Librarian_gridCategories.Children.Add((new GridRow(0, new View[] { Librarian_lblCategories }, null, false, false)).Row);
            Librarian_gridCategories.Children.Add((new GridRow(1, new View[] { Librarian_lvCategories }, null, false, false)).Row);
            Librarian_gridCategories.RowDefinitions = new RowDefinitionCollection();
            Librarian_gridCategories.RowDefinitions.Add(new RowDefinition());
            Librarian_gridCategories.RowDefinitions.Add(new RowDefinition());
            Librarian_gridCategories.RowDefinitions[0].Height = new GridLength(30, GridUnitType.Absolute);
            Librarian_gridCategories.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);

            // Assemble column 2:
            Librarian_gridTones.Children.Add((new GridRow(0, new View[] { Librarian_filterPresetAndUser }, null, false, false)).Row);
            Librarian_gridTones.Children.Add((new GridRow(1, new View[] { Librarian_lvToneNames }, null, false, false)).Row);
            Librarian_gridTones.RowDefinitions = new RowDefinitionCollection();
            Librarian_gridTones.RowDefinitions.Add(new RowDefinition());
            Librarian_gridTones.RowDefinitions.Add(new RowDefinition());
            Librarian_gridTones.RowDefinitions[0].Height = new GridLength(30, GridUnitType.Absolute);
            Librarian_gridTones.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);

            // Assemble column 3:
            Librarian_gridToneData.Children.Add((new GridRow(0, new View[] { Librarian_midiOutputDevice, Librarian_midiInputDevice, Librarian_midiOutputChannel, Librarian_midiInputChannel }, new byte[] { 255, 1, 255, 1 }, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(1, new View[] { Librarian_tbSearch }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(2, new View[] { Librarian_ltToneName }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(3, new View[] { Librarian_ltType }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(4, new View[] { Librarian_ltToneNumber }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(5, new View[] { Librarian_ltBankNumber }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(6, new View[] { Librarian_ltBankMSB }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(7, new View[] { Librarian_ltBankLSB }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(8, new View[] { Librarian_ltProgramNumber }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(9, new View[] { Librarian_btnEditTone, Librarian_btnEditStudioSet }, new byte[] { 1, 2 }, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(10, new View[] { Librarian_btnResetVolume, Librarian_btnMotionalSurround }, new byte[] { 1, 2 }, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(11, new View[] { Librarian_btnFavorites, Librarian_btnAddFavorite, Librarian_btnRemoveFavorite }, new byte[] { 1, 1, 1 }, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(12, new View[] { Librarian_btnResetHangingNotes, Librarian_btnPlay }, new byte[] { 1, 2 }, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(13, new View[] { Librarian_lblKeys, Librarian_btnMinus12keys, Librarian_btnPlus12keys }, new byte[] { 1, 1, 1 }, false)).Row);

            // Assemble LibrarianStackLayout --------------------------------------------------------------

            LibrarianStackLayout = new StackLayout();
            LibrarianStackLayout.Children.Add((new GridRow(0, new View[] { Librarian_gridGroups, Librarian_gridCategories, Librarian_gridTones, Librarian_gridToneData }, new byte[] { 1, 1, 1, 1 })).Row);
            LibrarianStackLayout.Children.Add((new GridRow(1, new View[] { Librarian_Keyboard })).Row);
            ((Grid)LibrarianStackLayout.Children[0]).RowDefinitions = new RowDefinitionCollection();
            ((Grid)LibrarianStackLayout.Children[0]).RowDefinitions.Add(new RowDefinition());
            ((Grid)LibrarianStackLayout.Children[0]).RowDefinitions.Add(new RowDefinition());
            ((Grid)LibrarianStackLayout.Children[0]).RowDefinitions[0].Height = new GridLength(8, GridUnitType.Star);
            ((Grid)LibrarianStackLayout.Children[0]).RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Librarian handlers
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Librarian_btnMinus12keys_Clicked(object sender, EventArgs e)
        {
            commonState.midi.NoteOff(0, 64);
        }

        private void Librarian_btnPlus12keys_Clicked(object sender, EventArgs e)
        {
            commonState.midi.NoteOn(0, 64, 64);
        }

        private void Librarian_btnResetHangingNotes_Clicked(object sender, EventArgs e)
        {
        }

        private void Librarian_btnFavorites_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowFavoritesPage(FavoriteAction.SHOW);
        }

        private void Librarian_btnPlay_Clicked(object sender, EventArgs e)
        {
        }

        private void Librarian_btnRemoveFavorite_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowFavoritesPage(FavoriteAction.REMOVE);
        }

        private void Librarian_btnAddFavorite_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowFavoritesPage(FavoriteAction.ADD);
        }

        private void Librarian_btnMotionalSurround_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowMotionalSurroundPage();
        }

        private void Librarian_btnResetVolume_Clicked(object sender, EventArgs e)
        {
        }

        private void Librarian_btnEditStudioSet_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowStudioSetEditorPage();
        }

        private void Librarian_Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (initDone)
            {
            }
        }

        private void Librarian_MidiOutputChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initDone)
            {
            }

        }

        private void Librarian_MidiOutputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initDone)
            {
            }

        }

        private void Librarian_LvToneNames_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (initDone)
            {
            }

        }

        private void Librarian_LvCategories_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (initDone && allowListViewUpdates)
            {
                allowListViewUpdates = false;
                PopulateToneNames(Librarian_lvCategories.SelectedItem.ToString());
                allowListViewUpdates = true;
            }

        }

        private void Librarian_LvGroups_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (initDone)
            {
                PopulateCategories(Librarian_lvGroups.SelectedItem.ToString());
            }

        }

        private void Librarian_BtnEditTone_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowToneEditorPage();
        }

        private void Librarian_FilterPresetAndUser_Clicked(object sender, EventArgs e)
        {
            switch (toneNamesFilter)
            {
                case ToneNamesFilter.ALL:
                    toneNamesFilter = ToneNamesFilter.PRESET;
                    Librarian_filterPresetAndUser.Text = "Preset tones only";
                    break;
                case ToneNamesFilter.PRESET:
                    toneNamesFilter = ToneNamesFilter.USER;
                    Librarian_filterPresetAndUser.Text = "User tones only";
                    break;
                case ToneNamesFilter.USER:
                    toneNamesFilter = ToneNamesFilter.ALL;
                    Librarian_filterPresetAndUser.Text = "Presets and user tones";
                    break;
            }
            PopulateToneNames(Librarian_lvCategories.SelectedItem.ToString());
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Librarian_ltToneName.Text.Text = (String)((Picker)(sender)).SelectedItem;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Librarian functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ShowLibrarianPage()
        {
            mainStackLayout.Children.Add(LibrarianStackLayout);
            if (Librarian_Groups.Count == 0)
            {
                PopulateGroups();
            }
            //PopulateCategories(commonState.currentTone.Group);
            //PopulateToneNames(commonState.currentTone.Category);
            //// Fill out form:
            //if (commonState.currentTone.Index > -1)
            //{
            //    PopulateToneData(commonState.currentTone.Index);
            //}
        }

        private void PopulateGroups()
        {
            allowListViewUpdates = false;
            foreach (List<String> tone in commonState.toneList.Tones)
            {
                if (!Librarian_Groups.Contains(tone[0]))
                {
                    Librarian_Groups.Add(tone[0]);
                }
            }
            allowListViewUpdates = true;
            Librarian_lvGroups.SelectedItem = "SuperNATURAL Acoustic Tone";
        }

        private void PopulateCategories(String group)
        {
            t.Trace("private void PopulateCategories (" + "String" + group + ", " + ")");
            //commonState.currentTone.Group = group;
            String lastCategory = "";
            allowListViewUpdates = false;
            Librarian_Categories.Clear();
            foreach (List<String> line in commonState.toneList.Tones.OrderBy(o => o[1]))
            {
                if (line[0] == group && line[1] != lastCategory && !Librarian_Categories.Contains(line[1]))
                {
                    Librarian_Categories.Add(line[1]);
                    lastCategory = line[1];
                }
            }
            allowListViewUpdates = true;
            Librarian_lvCategories.ItemsSource = Librarian_Categories;
            Librarian_lvCategories.SelectedItem = Librarian_Categories[0];
            //if (!String.IsNullOrEmpty(commonState.currentTone.Category))
            //{
            //    try
            //    {
            //        Librarian_lvCategories.SelectedItem = commonState.currentTone.Category;
            //    }
            //    catch { }
            //}
            PopulateToneNames(Librarian_lvCategories.SelectedItem.ToString());
        }

        private void PopulateToneNames(String category)
        {
            t.Trace("private void PopulateToneNames (" + "String" + category + ", " + ")");
            if (initDone || !scanning)
            {
                //commonState.currentTone.Category = category;

                try
                {
                    //if (TonesSource.Count() > 0)
                    if (Librarian_ToneNames == null)
                    {
                        try
                        {
                            Librarian_ToneNames = new ObservableCollection<String>();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    if (Librarian_ToneNames.Count() > 0)
                    {
                        try
                        {
                            Librarian_ToneNames.Clear();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    foreach (List<String> tone in commonState.toneList.Tones.OrderBy(o => o[3]))
                    {
                        if (commonState.currentTone == null && tone[1] == category)
                        {
                            Librarian_ToneNames.Add(tone[3]);
                        }
                        //else if (tone[0] == commonState.currentTone.Group && tone[1] == category
                        //    //&& ((Boolean)cbMainPageFilterUser.IsChecked && tone[8] == "(User)"
                        //    //|| (Boolean)cbMainPageFilterPreset.IsChecked && tone[8] != "(User)")
                        //    )
                        //{
                        //    Librarian_ToneNames.Add(tone[3]);
                        //}
                    }
                    if (!String.IsNullOrEmpty(commonState.currentTone.Name))
                    {
                        try
                        {
                            Librarian_lvToneNames.SelectedItem = commonState.currentTone.Name;
                            //if (IsFavorite())
                            //{
                            //    btnShowFavorites.Background = green;
                            //}
                            //else
                            //{
                            //    btnShowFavorites.Background = gray;
                            //}
                        }
                        catch
                        {
                            Librarian_lvToneNames.SelectedItem = "";
                        }
                    }
                    //else if (lvToneNames.Items.Count > 0)
                    //{
                    //    lvToneNames.SelectedIndex = 0;
                    //}
                }
                catch (Exception e)
                {

                }
                //SetFavorite();
                Librarian_lvToneNames.ItemsSource = Librarian_ToneNames;
            }
        }
    }
}
