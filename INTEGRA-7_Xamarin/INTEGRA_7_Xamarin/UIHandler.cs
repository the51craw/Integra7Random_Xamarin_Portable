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
        private HBTrace t = new HBTrace("public sealed partial class MainPage : Page");

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

        enum QueryType
        {
            NONE,
            CHECKING_I_7_READINESS,
            PCM_SYNTH_TONE_COMMON,
            PCM_SYNTH_TONE_COMMON2,
            PCM_DRUM_KIT_COMMON,
            SUPERNATURAL_ACOUSTIC_TONE_COMMON,
            SUPERNATURAL_SYNTH_TONE_COMMON,
            SUPERNATURAL_DRUM_KIT_COMMON,
            PCM_KEY_NAME,
            SND_KEY_NAME,
            CURRENT_SELECTED_STUDIO_SET,
            CURRENT_SELECTED_TONE,
        }

        Boolean scanAll = false;
        UInt16 emptySlots = 10;

        //ApplicationDataContainer localSettings = null;
        CommonState commonState = null;
        public List<List<String>> Lists = null;
        private Boolean AutoUpdateChildLists = true;
        private Int32 currentGroupIndex = -1;
        private Int32 currentCategoryIndex = -1;
        private Int32 currentToneNameIndex = -1;
        private byte currentNote = 255; // > 127 when note is off
        public static String[] lines;
        // private DrumKeyAssignLists drumKeyAssignLists = null; Moved to commonState
        private Int32 lowKey = 36;
        private Int32 transpose = 0;
        private byte[] notes = { 36, 40, 43, 48 };
        private byte[] drumNotes = { 36, 38, 42, 45, 43, 41,
            60, 61, 56, 69, 70, 54, 65, 76, 77,
            46, 44, 51, 53, 52, 67, 68, 69, 70 };
        private byte[] whiteKeys = { 36, 38, 40, 41, 43, 45,
            47, 48, 50, 52, 53, 55, 57, 59, 60, 62, 64, 65,
            67, 69, 71, 72, 74, 76, 77, 79, 81, 83, 84, 86,
            88, 89, 91, 93, 95, 96 };
        private Boolean initDone = false;
        private Boolean scanning = false;
        //private DispatcherTimer timer = null;
        private Boolean followOutputPort = false;
        private Boolean followingOutputPort = false;
        private Boolean waitingForInputPortChange = false;
        private String outputPortId;
        private Int32 savedInputPort = -1;
        private Int32 testingInputPortId;
        private byte msb;
        private byte lsb;
        private byte pc;
        private byte key;
        String toneName;
        String category;
        byte toneCategory;
        Int32 userToneIndex;
        Boolean integra_7Ready = false;
        byte integra_7ReadyCounter = 100;
        //SolidColorBrush green = null;
        //SolidColorBrush gray = null;
        Boolean initMidi = false;
        UInt16[] userToneNumbers;
        QueryType queryType;
        Boolean updateToneNames = false;
        ToneCategories toneCategories = new ToneCategories();
        Hex2Midi hex2Midi = new Hex2Midi();
        //SuperNATURALDrumKitInstrumentList superNATURALDrumKitInstrumentList = new SuperNATURALDrumKitInstrumentList();


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
            Init();
        }

        public void Init()
        {
            page = _page.LIBRARIAN;
            colorSettings = new ColorSettings(_colorSettings.LIGHT);
            borderThicknesSettings = new BorderThicknesSettings(1);
            commonState = new CommonState(ref Librarian_btnPlay);
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

            GotoLibrarian();
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

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Page swapping functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void GotoLibrarian()
        {
            mainStackLayout.Children.Add(LibrarianStackLayout);
            foreach (List<String> tone in commonState.toneList.Tones)
            {
                if (!Librarian_Groups.Contains(tone[0]))
                {
                    Librarian_Groups.Add(tone[0]);
                }
            }
            //PopulateCategories(commonState.currentTone.Group);
            //PopulateToneNames(commonState.currentTone.Category);
            //// Fill out form:
            //if (commonState.currentTone.Index > -1)
            //{
            //    PopulateToneData(commonState.currentTone.Index);
            //}
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Librarian functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void PopulateCategories(String group)
        {
            t.Trace("private void PopulateCategories (" + "String" + group + ", " + ")");
            commonState.currentTone.Group = group;
            String lastCategory = "";
            //CategoriesSource.Clear();
            //lvCategories.Items.Clear();
            //foreach (List<String> line in commonState.toneList.Tones.OrderBy(o => o[1]))
            //{
            //    //if (line[0] == group && line[1] != lastCategory && !CategoriesSource.Contains(line[1]))
            //    if (line[0] == group && line[1] != lastCategory && !lvCategories.Items.Contains(line[1]))
            //    {
            //        lvCategories.Items.Add(line[1]);
            //        //CategoriesSource.Add(line[1]);
            //        lastCategory = line[1];
            //    }
            //}
            //if (!String.IsNullOrEmpty(commonState.currentTone.Category))
            //{
            //    try
            //    {
            //        lvCategories.SelectedItem = commonState.currentTone.Category;
            //    }
            //    catch { }
            //}
        }


    }
}
