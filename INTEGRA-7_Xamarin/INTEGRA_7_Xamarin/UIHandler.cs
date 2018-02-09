using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    /**
     * All pages are dynamically created here.
     * They all exists simultaneously, but shown one at a time by assigning it to mainStackLayout.
     * Some pages has subpages that are generated in MakeDynamicControls.cs, Help.cs and
     * ControlHandlers.cs
     */
    public class UIHandler
    {
        public enum _appType
        {
            UWP,
            IOS,
            MacOS,
            ANDROID,
        }

        enum _page
        {
            LIBRARIAN,
            SEARCH_RESULTS,
            FAVORITES,
            EDIT,
        }

        IMidi midi;

        INTEGRA_7_Xamarin.MainPage mainPage;
        StackLayout mainStackLayout { get; set; }
        StackLayout LibrarianStackLayout;
        StackLayout EditorStackLayout;

        public static _appType appType;
        public static ColorSettings colorSettings { get; set; }
        public static BorderThicknesSettings borderThicknesSettings { get; set; }
        _page page;
        Boolean InitDone = false;

        // Librarian controls:
        public Picker Librarian_midiOutputDevice { get; set; }
        public Picker Librarian_midiInputDevice { get; set; }
        public Picker Librarian_midiOutputChannel { get; set; }
        public Picker Librarian_midiInputChannel { get; set; }
        ListView Librarian_lvGroups;
        List<String> Librarian_Groups;
        ListView Librarian_lvCategories;
        List<String> Librarian_Categories;
        Grid Librarian_gridTones;
        Button Librarian_filterPresetAndUser;
        ListView Librarian_lvToneNames;
        List<String> Librarian_ToneNames;
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

        // Edit tone controls:
        //...

        // Constructor
        public UIHandler(StackLayout mainStackLayout, INTEGRA_7_Xamarin.MainPage mainPage)
        {
            this.mainStackLayout = mainStackLayout;
            this.mainPage = mainPage;
            page = _page.LIBRARIAN;
            colorSettings = new ColorSettings(_colorSettings.LIGHT);
            borderThicknesSettings = new BorderThicknesSettings(1);
        }

        public void Clear()
        {
            while (mainStackLayout.Children.Count() > 0)
            {
                mainStackLayout.Children.RemoveAt(0);
            }
        }

        // Creates all pages and shows the librarian:
        public void DrawPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Librarian 
            //                                                           ____________________________________ gridTones
            //                                                          /                      ______________ gridToneData
            //                                                         /                      /
            // ____________________________________________________________________________________________
            // | lvGroups             | lvCategories         | filterPresetAndUser |midiOutputDevice/part |
            // |                      |                      |---------------------|----------------------|
            // |                      |                      | lvToneNames         |tbSearch              |
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
            Librarian_lvGroups = new ListView();
            Librarian_Groups = new List<String>();
            Librarian_lvGroups.ItemsSource = Librarian_Groups;

            // Make a listview lvCategories for column 1:
            Librarian_lvCategories = new ListView();
            Librarian_Categories = new List<String>();
            Librarian_lvCategories.ItemsSource = Librarian_Categories;

            // Make a Grid for column 2:
            Librarian_gridTones = new Grid();

            // Make a filter button for column 2:
            Librarian_filterPresetAndUser = new Button();
            Librarian_filterPresetAndUser.Text = "Preset and User";
            Librarian_filterPresetAndUser.BackgroundColor = colorSettings.Background;

            // Make a listview lvToneNames for column 2:
            Librarian_lvToneNames = new ListView();
            Librarian_lvToneNames.BackgroundColor = colorSettings.Background;
            Librarian_ToneNames = new List<String>();
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

            // Assemble grids with controls ---------------------------------------------------------------

            // Columns 0 and 1 are only one listview each and thus has nothing to assemble:
            // lvGroups and lvCategories 

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
            LibrarianStackLayout.Children.Add((new GridRow(0, new View[] { Librarian_lvGroups, Librarian_lvCategories, Librarian_gridTones, Librarian_gridToneData }, new byte[] { 1, 1, 1, 1 })).Row);
            LibrarianStackLayout.Children.Add((new GridRow(1, new View[] { Librarian_Keyboard })).Row);
            ((Grid)LibrarianStackLayout.Children[0]).RowDefinitions = new RowDefinitionCollection();
            ((Grid)LibrarianStackLayout.Children[0]).RowDefinitions.Add(new RowDefinition());
            ((Grid)LibrarianStackLayout.Children[0]).RowDefinitions.Add(new RowDefinition());
            ((Grid)LibrarianStackLayout.Children[0]).RowDefinitions[0].Height = new GridLength(8, GridUnitType.Star);
            ((Grid)LibrarianStackLayout.Children[0]).RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Edit tone  
            //                                                           ____________________________________ gridTones
            //                                                          /                      ______________ gridToneData
            //                                                         /                      /
            // ____________________________________________________________________________________________
            // | lvGroups             | lvCategories         | filterPresetAndUser |midiOutputDevice/part |
            // |                      |                      |---------------------|----------------------|
            // |                      |                      | lvToneNames         |tbSearch              |
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

            Button test = new Button();
            test.Text = "test";
            test.Clicked += Test_Clicked;

            // Add handlers -------------------------------------------------------------------------------

            // Assemble grids with controls ---------------------------------------------------------------

            // Assemble EditorStackLayout -----------------------------------------------------------------

            EditorStackLayout = new StackLayout();
            EditorStackLayout.Children.Add((new GridRow(0, new View[] { test })).Row);

            // Assemble LibrarianStackLayout --------------------------------------------------------------

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Edit studio set
            // ____________________________________________________________________________________________
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Create all controls ------------------------------------------------------------------------

            // Add handlers -------------------------------------------------------------------------------

            // Assemble grids with controls ---------------------------------------------------------------

            // Assemble EditorStackLayout -----------------------------------------------------------------

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Surround
            // ____________________________________________________________________________________________
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Create all controls ------------------------------------------------------------------------

            // Add handlers -------------------------------------------------------------------------------

            // Assemble grids with controls ---------------------------------------------------------------

            // Assemble EditorStackLayout -----------------------------------------------------------------

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Favorites
            // ____________________________________________________________________________________________
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Create all controls ------------------------------------------------------------------------

            // Add handlers -------------------------------------------------------------------------------

            // Assemble grids with controls ---------------------------------------------------------------

            // Assemble EditorStackLayout -----------------------------------------------------------------

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Always start by showing ligrarian:

            mainStackLayout.Children.Add(LibrarianStackLayout);
        }

        private void Librarian_Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (InitDone)
            {
            }
        }

        private void Librarian_MidiOutputChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InitDone)
            {
            }

        }

        private void Librarian_MidiOutputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InitDone)
            {
            }

        }

        private void Librarian_LvToneNames_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (InitDone)
            {
            }

        }

        private void Librarian_LvCategories_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (InitDone)
            {
            }

        }

        private void Librarian_LvGroups_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (InitDone)
            {
            }

        }

        private void Test_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            mainStackLayout.Children.Add(LibrarianStackLayout);
        }

        private void Librarian_BtnEditTone_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            mainStackLayout.Children.Add(EditorStackLayout);
        }

        private void Librarian_FilterPresetAndUser_Clicked(object sender, EventArgs e)
        {
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Librarian_ltToneName.Text.Text = (String)((Picker)(sender)).SelectedItem;
        }
    }
}
