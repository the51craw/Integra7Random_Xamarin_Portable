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
            USER = 2,
            INIT = 3,
        }
        ToneNamesFilter toneNamesFilter = ToneNamesFilter.INIT;

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
        //ListView Librarian_lvSearchResult;
        //ObservableCollection<String> Librarian_SearchResult;
        Grid Librarian_gridToneData;
        LabeledTextInput Librarian_tbSearch;
        LabeledText Librarian_ltToneName;
        LabeledText Librarian_ltType;
        LabeledText Librarian_ltToneNumber;
        LabeledText Librarian_ltPatchNumber;
        LabeledText Librarian_ltPatchMSB;
        LabeledText Librarian_ltPatchLSB;
        LabeledText Librarian_ltProgramNumber;
        Button Librarian_btnEditTone;
        Button Librarian_btnEditStudioSet;
        Button Librarian_btnResetVolume;
        Button Librarian_btnMotionalSurround;
        Button Librarian_btnAddFavorite;
        Button Librarian_btnRemoveFavorite;
        Button Librarian_btnPlay;
        Button Librarian_btnShowFavorites;
        Button Librarian_btnResetHangingNotes;
        Button Librarian_btnPlus12keys;
        Button Librarian_btnMinus12keys;
        MyLabel Librarian_lblKeys;
        Image Librarian_Keyboard;
        Boolean usingSearchResults = false;

        SuperNATURALDrumKitInstrumentList superNATURALDrumKitInstrumentList = new SuperNATURALDrumKitInstrumentList();

        public void Librarian_Init()
        {
            //t.Trace("private void Librarian_Init()");
            //localSettings = ApplicationData.Current.LocalSettings;
            //green = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 155, 232, 130));
            //gray = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 204, 204, 204));
            //userToneNumbers = new UInt16[128];
            //for (byte i = 0; i < 128; i++)
            //{
            //    userToneNumbers[i] = 0;
            //}
        }

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
            // | lvGroups             | lvCategories         | lvToneNames or      |tbSearch              |
            // |                      |                      | lvSearchResult      |----------------------|
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
            Librarian_filterPresetAndUser.Text = "Load user tones";
            Librarian_filterPresetAndUser.BackgroundColor = colorSettings.Background;

            // Make listviews lvToneNames and lvSearchResult for column 2:
            Librarian_lvToneNames = new ListView();
            Librarian_lvToneNames.BackgroundColor = colorSettings.Background;
            Librarian_Categories = new ObservableCollection<String>();
            Librarian_lvToneNames.ItemsSource = Librarian_ToneNames;
            //Librarian_lvSearchResult = new ListView();
            //Librarian_lvSearchResult.BackgroundColor = colorSettings.Background;
            //Librarian_SearchResult = new ObservableCollection<String>();
            //Librarian_lvSearchResult.ItemsSource = Librarian_ToneNames;
            //Librarian_lvSearchResult.IsVisible = false;

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
            Librarian_ltToneName = new LabeledText("Tone Name:", "", new byte[] { 1, 2 });
            Librarian_ltType = new LabeledText("Type:", "", new byte[] { 1, 2 });
            Librarian_ltToneNumber = new LabeledText("Tone #:", "", new byte[] { 1, 2 });
            Librarian_ltPatchNumber = new LabeledText("Bank #:", "", new byte[] { 1, 2 });
            Librarian_ltPatchMSB = new LabeledText("Bank MSB:", "", new byte[] { 1, 2 });
            Librarian_ltPatchLSB = new LabeledText("Bank LSB:", "", new byte[] { 1, 2 });
            Librarian_ltProgramNumber = new LabeledText("Program #:", "", new byte[] { 1, 2 });

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
            Librarian_btnShowFavorites = new Button();
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
            Librarian_btnShowFavorites.Text = "Favorites";
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
            Librarian_btnShowFavorites.BackgroundColor = colorSettings.Background;
            Librarian_btnResetHangingNotes.BackgroundColor = colorSettings.Background;
            Librarian_btnPlus12keys.BackgroundColor = colorSettings.Background;
            Librarian_btnMinus12keys.BackgroundColor = colorSettings.Background;
            Librarian_lblKeys.BackgroundColor = colorSettings.Background;

            // Add handlers -------------------------------------------------------------------------------

            Librarian_lvGroups.ItemSelected += Librarian_LvGroups_ItemSelected;
            Librarian_lvCategories.ItemSelected += Librarian_LvCategories_ItemSelected;
            Librarian_filterPresetAndUser.Clicked += Librarian_FilterPresetAndUser_Clicked;
            Librarian_lvToneNames.ItemSelected += Librarian_LvToneNames_ItemSelected;
            //Librarian_lvSearchResult.ItemSelected += Librarian_lvSearchResult_ItemSelected;
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
            Librarian_btnShowFavorites.Clicked += Librarian_btnFavorites_Clicked;
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
            //Librarian_gridTones.Children.Add((new GridRow(2, new View[] { Librarian_lvSearchResult }, null, false, false)).Row);
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
            Librarian_gridToneData.Children.Add((new GridRow(5, new View[] { Librarian_ltPatchNumber }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(6, new View[] { Librarian_ltPatchMSB }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(7, new View[] { Librarian_ltPatchLSB }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(8, new View[] { Librarian_ltProgramNumber }, null, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(9, new View[] { Librarian_btnEditTone, Librarian_btnEditStudioSet }, new byte[] { 1, 2 }, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(10, new View[] { Librarian_btnResetVolume, Librarian_btnMotionalSurround }, new byte[] { 1, 2 }, false)).Row);
            Librarian_gridToneData.Children.Add((new GridRow(11, new View[] { Librarian_btnShowFavorites, Librarian_btnAddFavorite, Librarian_btnRemoveFavorite }, new byte[] { 1, 1, 1 }, false)).Row);
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

        public void Librarian_MidiInPort_MessageReceived()
        {
            //t.Trace("private void MainPage_MidiInPort_MessageReceived");
            if (queryType == QueryType.CHECKING_I_7_READINESS)
            {
                integra_7Ready = true;
            }
            else if (initDone || scanning)
            {
                try
                {
                    if (rawData[0] == 0xf0) // handle system exclusive messages only
                    {
                        byte[] data = rawData;
                        switch (queryType)
                        {
                            case QueryType.PCM_SYNTH_TONE_COMMON:
                                // This is the first user tone.
                                // Its index should be set to continue after the preset tones:
                                userToneIndex = commonState.toneList.PresetsCount;
                                if (!IsInitTone(data))
                                {
                                    toneName = "";
                                    for (byte i = 0; i < 12; i++)
                                    {
                                        toneName += (char)data[i + 11];
                                    }
                                    toneName = toneName.Trim();
                                    commonState.toneNames[0].Add(toneName);
                                    // Also read common2 to get tone category:
                                    byte[] address = new byte[] { 0x1c, 0x60, 0x30, 0x00 };
                                    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x3c };
                                    byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
                                    commonState.midi.SendSystemExclusive(message);
                                    queryType = QueryType.PCM_SYNTH_TONE_COMMON2;
                                }
                                else
                                {
                                    commonState.toneNames[0].Add("INIT TONE");
                                    emptySlots++;
                                    pc++;
                                    if (pc > 128 || (!scanAll && emptySlots > 10))
                                    {
                                        lsb++;
                                        pc = 1;
                                        if (lsb > 1)
                                        {
                                            // No more patches to test!
                                            while (commonState.toneNames[0].Count() < 256)
                                            {
                                                commonState.toneNames[0].Add("INIT TONE");
                                            }
                                            msb = 86;
                                            lsb = 0;
                                            pc = 1;
                                            emptySlots = 0;
                                            QueryUserPCMDrumKitTones();
                                            break;
                                        }
                                    }
                                    // Check next:
                                    QueryUserPCMSyntTones();
                                }
                                break;
                            case QueryType.PCM_SYNTH_TONE_COMMON2:
                                toneCategory = data[0x1b];
                                List<String> tone = new List<String>();
                                tone.Add("PCM Synth Tone");
                                tone.Add(toneCategories.pcmToneCategoryNames[toneCategory]);
                                tone.Add((userToneNumbers[toneCategory]++).ToString());
                                tone.Add(toneName);
                                tone.Add(msb.ToString());
                                tone.Add(lsb.ToString());
                                tone.Add((msb * 128 + lsb).ToString());
                                tone.Add(pc.ToString());
                                tone.Add("(User)");
                                tone.Add((userToneIndex++).ToString());
                                commonState.toneList.Tones.Add(tone);
                                pc++;
                                if (pc > 128 || (!scanAll && emptySlots > 10))
                                {
                                    lsb++;
                                    pc = 1;
                                    if (lsb > 1 || (!scanAll && emptySlots > 10))
                                    {
                                        // No more patches to test!
                                        while (commonState.toneNames[0].Count() < 256)
                                        {
                                            commonState.toneNames[0].Add("INIT TONE");
                                        }
                                        msb = 86;
                                        lsb = 0;
                                        pc = 1;
                                        emptySlots = 10;
                                        for (byte i = 0; i < 128; i++)
                                        {
                                            userToneNumbers[i] = 0;
                                        }
                                        QueryUserPCMDrumKitTones();
                                        break;
                                    }
                                }
                                // Check next:
                                QueryUserPCMSyntTones();
                                break;
                            case QueryType.PCM_DRUM_KIT_COMMON:
                                if (!IsInitKit(data))
                                {
                                    toneName = "";
                                    for (byte i = 0; i < 12; i++)
                                    {
                                        toneName += (char)data[i + 11];
                                    }
                                    toneName = toneName.Trim();
                                    commonState.toneNames[1].Add(toneName);
                                    tone = new List<String>();
                                    tone.Add("PCM Drum Kit");
                                    tone.Add("Drum");
                                    tone.Add((userToneNumbers[toneCategory]++).ToString());
                                    tone.Add(toneName);
                                    tone.Add(msb.ToString());
                                    tone.Add(lsb.ToString());
                                    tone.Add((msb * 128 + lsb).ToString());
                                    tone.Add(pc.ToString());
                                    tone.Add("(User)");
                                    tone.Add((userToneIndex++).ToString());
                                    commonState.toneList.Tones.Add(tone);
                                    // Create a list for the key names:
                                    commonState.drumKeyAssignLists.ToneNames.Add(new List<String>());
                                    commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1].Add("PCM Drum Kit");
                                    commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1].Add(toneName);
                                    // Read all key names:
                                    key = 0;
                                    QueryPcmDrumKitKeyName(key);
                                    break;
                                }
                                else
                                {
                                    commonState.toneNames[1].Add("INIT KIT");
                                    emptySlots++;
                                }
                                pc++;
                                if (pc > 32 || (!scanAll && emptySlots > 10))
                                {
                                    // No more patches to test!
                                    while (commonState.toneNames[1].Count() < 32)
                                    {
                                        commonState.toneNames[1].Add("INIT KIT");
                                    }
                                    msb = 89;
                                    lsb = 0;
                                    pc = 1;
                                    emptySlots = 10;
                                    for (byte i = 0; i < 128; i++)
                                    {
                                        userToneNumbers[i] = 0;
                                    }
                                    emptySlots = 0;
                                    QueryUserSuperNaturalAcousticTones();
                                    break;
                                }
                                // Check next:
                                QueryUserPCMDrumKitTones();
                                break;
                            case QueryType.PCM_KEY_NAME:
                                // Put the name into the list:
                                String name = "";
                                for (byte i = 0; i < 12; i++)
                                {
                                    name += (char)data[i + 11];
                                }
                                commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1].Add(name);
                                // Query next if more is expected:
                                key++;
                                if (key < 88)
                                {
                                    // Query next key:
                                    QueryPcmDrumKitKeyName(key);
                                }
                                else
                                {
                                    // Query next PCM Drum Kit:
                                    pc++;
                                    if (pc > 32 || (!scanAll && emptySlots > 10))
                                    {
                                        // No more patches to test!
                                        while (commonState.toneNames[1].Count() < 32)
                                        {
                                            commonState.toneNames[1].Add("INIT KIT");
                                        }
                                        msb = 89;
                                        lsb = 0;
                                        pc = 1;
                                        emptySlots = 10;
                                        for (byte i = 0; i < 128; i++)
                                        {
                                            userToneNumbers[i] = 0;
                                        }
                                        emptySlots = 0;
                                        QueryUserSuperNaturalAcousticTones();
                                        break;
                                    }
                                    // Check next:
                                    QueryUserPCMDrumKitTones();
                                }
                                break;
                            case QueryType.SUPERNATURAL_ACOUSTIC_TONE_COMMON:
                                if (!IsInitTone(data))
                                {
                                    toneCategory = data[0x26];
                                    toneName = "";
                                    for (byte i = 0; i < 12; i++)
                                    {
                                        toneName += (char)data[i + 11];
                                    }
                                    toneName = toneName.Trim();
                                    commonState.toneNames[2].Add(toneName);
                                    tone = new List<String>();
                                    tone.Add("SuperNATURAL Acoustic Tone");
                                    tone.Add(toneCategories.snaToneCategoryNames[toneCategory]);
                                    tone.Add((userToneNumbers[toneCategory]++).ToString());
                                    tone.Add(toneName);
                                    tone.Add(msb.ToString());
                                    tone.Add(lsb.ToString());
                                    tone.Add((msb * 128 + lsb).ToString());
                                    tone.Add(pc.ToString());
                                    tone.Add("(User)");
                                    tone.Add((userToneIndex++).ToString());
                                    commonState.toneList.Tones.Add(tone);
                                }
                                else
                                {
                                    commonState.toneNames[2].Add("INIT TONE");
                                    emptySlots++;
                                }
                                pc++;
                                if (pc > 128 || (!scanAll && emptySlots > 10))
                                {
                                    lsb++;
                                    pc = 1;
                                    if (lsb > 1 || (!scanAll && emptySlots > 10))
                                    {
                                        // No more patches to test!
                                        while (commonState.toneNames[2].Count() < 256)
                                        {
                                            commonState.toneNames[2].Add("INIT TONE");
                                        }
                                        msb = 95;
                                        lsb = 0;
                                        pc = 1;
                                        emptySlots = 10;
                                        for (byte i = 0; i < 128; i++)
                                        {
                                            userToneNumbers[i] = 0;
                                        }
                                        emptySlots = 0;
                                        QueryUserSuperNaturalSynthTones();
                                        break;
                                    }
                                }
                                // Check next:
                                QueryUserSuperNaturalAcousticTones();
                                break;
                            case QueryType.SUPERNATURAL_SYNTH_TONE_COMMON:
                                if (!IsInitTone(data))
                                {
                                    toneCategory = data[0x41];
                                    toneName = "";
                                    for (byte i = 0; i < 12; i++)
                                    {
                                        toneName += (char)data[i + 11];
                                    }
                                    toneName = toneName.Trim();
                                    commonState.toneNames[3].Add(toneName);
                                    tone = new List<String>();
                                    tone.Add("SuperNATURAL Synth Tone");
                                    tone.Add(toneCategories.snsToneCategoryNames[toneCategory]);
                                    tone.Add((userToneNumbers[toneCategory]++).ToString());
                                    tone.Add(toneName);
                                    tone.Add(msb.ToString());
                                    tone.Add(lsb.ToString());
                                    tone.Add((msb * 128 + lsb).ToString());
                                    tone.Add(pc.ToString());
                                    tone.Add("(User)");
                                    tone.Add((userToneIndex++).ToString());
                                    commonState.toneList.Tones.Add(tone);
                                }
                                else
                                {
                                    commonState.toneNames[3].Add("INIT TONE");
                                    emptySlots++;
                                }
                                pc++;
                                if (pc > 128 || (!scanAll && emptySlots > 10))
                                {
                                    lsb++;
                                    pc = 1;
                                    if (lsb > 3 || (!scanAll && emptySlots > 10))
                                    {
                                        // No more patches to test!
                                        while (commonState.toneNames[3].Count() < 512)
                                        {
                                            commonState.toneNames[3].Add("INIT TONE");
                                        }
                                        msb = 88;
                                        lsb = 0;
                                        pc = 1;
                                        emptySlots = 10;
                                        for (byte i = 0; i < 128; i++)
                                        {
                                            userToneNumbers[i] = 0;
                                        }
                                        emptySlots = 0;
                                        QueryUserSuperNaturalDrumKitTones();
                                        break;
                                    }
                                }
                                // Check next:
                                QueryUserSuperNaturalSynthTones();
                                break;
                            case QueryType.SUPERNATURAL_DRUM_KIT_COMMON:
                                if (!IsInitKit(data))
                                {
                                    toneName = "";
                                    for (byte i = 0; i < 12; i++)
                                    {
                                        toneName += (char)data[i + 11];
                                    }
                                    toneName = toneName.Trim();
                                    commonState.toneNames[4].Add(toneName);
                                    tone = new List<String>();
                                    tone.Add("SuperNATURAL Drum Kit");
                                    tone.Add("Drum");
                                    tone.Add((userToneNumbers[toneCategory]++).ToString());
                                    tone.Add(toneName);
                                    tone.Add(msb.ToString());
                                    tone.Add(lsb.ToString());
                                    tone.Add((msb * 128 + lsb).ToString());
                                    tone.Add(pc.ToString());
                                    tone.Add("(User)");
                                    tone.Add((userToneIndex++).ToString());
                                    commonState.toneList.Tones.Add(tone);
                                    // Create a list for the key names:
                                    commonState.drumKeyAssignLists.ToneNames.Add(new List<String>());
                                    commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1].Add("SuperNATURAL Drum Kit");
                                    commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1].Add(toneName);
                                    // SN-D keys does not have keys 22 - 26, fill with empth slots:
                                    commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1].Add("-----");
                                    commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1].Add("-----");
                                    commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1].Add("-----");
                                    commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1].Add("-----");
                                    commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1].Add("-----");
                                    // Read all key names:
                                    key = 0;
                                    QuerySnDrumKitKeyName(key);
                                    break;
                                }
                                else
                                {
                                    commonState.toneNames[4].Add("INIT KIT");
                                    emptySlots++;
                                }
                                pc++;
                                if (pc > 64 || (!scanAll && emptySlots > 10))
                                {
                                    // No more patches to test!
                                    while (commonState.toneNames[4].Count() < 64)
                                    {
                                        commonState.toneNames[4].Add("INIT KIT");
                                    }
                                    QuerySelectedStudioSet();
                                    break;
                                }
                                // Check next:
                                QueryUserSuperNaturalDrumKitTones();
                                break;
                            case QueryType.SND_KEY_NAME:
                                // Put the name into the list:
                                try
                                {
                                    commonState.drumKeyAssignLists.ToneNames[commonState.drumKeyAssignLists.ToneNames.Count - 1]
                                        .Add(superNATURALDrumKitInstrumentList.DrumInstruments[data[12] * 256 + data[13] * 16 + data[14]].Name);
                                }
                                catch { }
                                // Query next if more is expected:
                                key++;
                                if (key < 61)
                                {
                                    // Query next key:
                                    QuerySnDrumKitKeyName(key);
                                }
                                else
                                {
                                    // Query next SN Drum Kit:
                                    pc++;
                                    if (pc > 64 || (!scanAll && emptySlots > 10))
                                    {
                                        // No more patches to test!
                                        while (commonState.toneNames[1].Count() < 32)
                                        {
                                            commonState.toneNames[1].Add("INIT KIT");
                                        }
                                        msb = 85;
                                        lsb = 0;
                                        pc = 1;
                                        emptySlots = 10;
                                        for (byte i = 0; i < 128; i++)
                                        {
                                            userToneNumbers[i] = 0;
                                        }
                                        QuerySelectedStudioSet();
                                        break;
                                    }
                                    // Check next:
                                    QueryUserSuperNaturalDrumKitTones();
                                }
                                break;
                            case QueryType.CURRENT_SELECTED_STUDIO_SET:
                                //commonState.CurrentStudioSet = receivedMidiMessage.RawData.ToArray()[0x11];
                                commonState.CurrentStudioSet = rawData[0x11];
                                QuerySelectedTone();
                                break;
                            case QueryType.CURRENT_SELECTED_TONE:
                                Int32 index = commonState.toneList.Get(
                                    rawData[0x11],
                                    rawData[0x12],
                                    (byte)(rawData[0x13]));
                                try
                                {
                                    commonState.currentTone = new Tone(commonState.toneList.Tones[index]);
                                }
                                catch { }
                                queryType = QueryType.NONE;
                                scanning = false;
                                updateToneNames = true;
                                break;
                        }
                    }
                }
                catch {}
            }
        }

        private void Librarian_LvGroups_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (initDone)
            {
                PopulateCategories(Librarian_lvGroups.SelectedItem.ToString());
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

        private void Librarian_LvToneNames_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (initDone)
            {
                if (usingSearchResults)
                {
                    //t.Trace("private void lvSearchResults_SelectionChanged (" + "object" + sender + ", " + "SelectionChangedEventArgs" + e + ", " + ")");
                    String soundName = (String)((ListView)sender).SelectedItem;
                    Boolean drumMap = false;
                    if (!String.IsNullOrEmpty(soundName))
                    {
                        commonState.currentTone.Name = soundName;
                    }
                    if (!String.IsNullOrEmpty(Librarian_tbSearch.Editor.Text))
                    {
                        if (commonState.currentTone.Name.EndsWith("\t"))
                        {
                            drumMap = true;
                            commonState.currentTone.Name = commonState.currentTone.Name.TrimEnd('\t');
                        }
                        String[] parts = commonState.currentTone.Name.Split(',');
                        if (parts.Length == 3)
                        {
                            if (drumMap)
                            {
                                commonState.currentTone.Group = parts[1].TrimStart();
                                commonState.currentTone.Category = "Drum";
                                commonState.currentTone.Name = parts[2].TrimStart();
                            }
                            else
                            {
                                commonState.currentTone.Group = parts[1].TrimStart();
                                commonState.currentTone.Category = parts[2].TrimStart();
                                commonState.currentTone.Name = parts[0].TrimStart();
                            }
                            Librarian_lvGroups.SelectedItem = commonState.currentTone.Group;
                            Librarian_lvCategories.SelectedItem = commonState.currentTone.Category;
                            Librarian_lvToneNames.SelectedItem = commonState.currentTone.Name;
                            commonState.currentTone.Index = commonState.toneList.Get(Librarian_lvGroups.SelectedItem.ToString(), Librarian_lvCategories.SelectedItem.ToString(), toneName);
                        }
                    }
                    if (!String.IsNullOrEmpty(commonState.currentTone.Name))
                    {
                        try
                        {
                            PopulateToneData(commonState.toneList.Get(commonState.currentTone.Group, commonState.currentTone.Category, commonState.currentTone.Name));
                        }
                        catch { }
                        commonState.midi.SetVolume(commonState.CurrentPart, 127);
                        UpdateDrumNames();
                        if (commonState.player.Playing)
                        {
                            commonState.player.StopPlaying();
                            commonState.player.StartPlaying();
                            commonState.player.WasPlaying = true;
                        }
                    }
                    Librarian_lvGroups.SelectedItem = commonState.currentTone.Group;
                }
                else
                {
                    //t.Trace("private void lvToneNames_SelectionChanged (" + "object" + sender + ", " + "SelectionChangedEventArgs" + e + ", " + ")");
                    if (initDone && !scanning)
                    {
                        if (Librarian_lvToneNames.SelectedItem != null && Librarian_lvToneNames.SelectedItem.ToString() != "")
                        {
                            //localSettings.Values["Tone"] = lvToneNames.SelectedIndex;
                            currentToneNameIndex = Librarian_ToneNames.IndexOf(Librarian_lvToneNames.SelectedItem.ToString());
                            toneName = (String)Librarian_lvToneNames.SelectedItem;
                            commonState.currentTone = new Tone(
                                Librarian_Groups.IndexOf(Librarian_lvGroups.SelectedItem.ToString()),
                                Librarian_Categories.IndexOf(Librarian_lvCategories.SelectedItem.ToString()),
                                Librarian_ToneNames.IndexOf(Librarian_lvToneNames.SelectedItem.ToString()),
                                Librarian_lvGroups.SelectedItem.ToString(),
                                Librarian_lvCategories.SelectedItem.ToString(), toneName);
                            if (!String.IsNullOrEmpty(toneName))
                            {
                                commonState.currentTone.Name = toneName;
                            }
                            if (!String.IsNullOrEmpty(commonState.currentTone.Name))
                            {
                                try
                                {
                                    if (commonState.currentTone.Index < 0)
                                    {
                                        commonState.currentTone.Index = commonState.toneList.Get(commonState.currentTone);
                                    }
                                    PopulateToneData(commonState.currentTone.Index);
                                }
                                catch { }
                                commonState.midi.SetVolume(commonState.CurrentPart, 127);
                                UpdateDrumNames();
                                if (commonState.player.Playing)
                                {
                                    commonState.player.StopPlaying();
                                    commonState.player.StartPlaying();
                                    commonState.player.WasPlaying = true;
                                }
                            }
                            //Librarian_lvGroups.SelectedItem = commonState.currentTone.Group;
                        }
                    }
                }
            }
        }

        //private void Librarian_lvSearchResult_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if (initDone)
        //    {
        //        //t.Trace("private void lvSearchResults_SelectionChanged (" + "object" + sender + ", " + "SelectionChangedEventArgs" + e + ", " + ")");
        //        String soundName = (String)((ListView)sender).SelectedItem;
        //        Boolean drumMap = false;
        //        if (!String.IsNullOrEmpty(soundName))
        //        {
        //            commonState.currentTone.Name = soundName;
        //        }
        //        if (!String.IsNullOrEmpty(Librarian_tbSearch.Editor.Text))
        //        {
        //            if (commonState.currentTone.Name.EndsWith("\t"))
        //            {
        //                drumMap = true;
        //                commonState.currentTone.Name = commonState.currentTone.Name.TrimEnd('\t');
        //            }
        //            String[] parts = commonState.currentTone.Name.Split(',');
        //            if (parts.Length == 3)
        //            {
        //                if (drumMap)
        //                {
        //                    commonState.currentTone.Group = parts[1].TrimStart();
        //                    commonState.currentTone.Category = "Drum";
        //                    commonState.currentTone.Name = parts[2].TrimStart();
        //                }
        //                else
        //                {
        //                    commonState.currentTone.Group = parts[1].TrimStart();
        //                    commonState.currentTone.Category = parts[2].TrimStart();
        //                    commonState.currentTone.Name = parts[0].TrimStart();
        //                }
        //                Librarian_lvGroups.SelectedItem = commonState.currentTone.Group;
        //                Librarian_lvCategories.SelectedItem = commonState.currentTone.Category;
        //                Librarian_lvToneNames.SelectedItem = commonState.currentTone.Name;
        //                commonState.currentTone.Index = commonState.toneList.Get(Librarian_lvGroups.SelectedItem.ToString(), Librarian_lvCategories.SelectedItem.ToString(), toneName);
        //            }
        //        }
        //        if (!String.IsNullOrEmpty(commonState.currentTone.Name))
        //        {
        //            try
        //            {
        //                PopulateToneData(commonState.toneList.Get(commonState.currentTone.Group, commonState.currentTone.Category, commonState.currentTone.Name));
        //            }
        //            catch { }
        //            commonState.midi.SetVolume(commonState.CurrentPart, 127);
        //            UpdateDrumNames();
        //            if (commonState.player.Playing)
        //            {
        //                commonState.player.StopPlaying();
        //                commonState.player.StartPlaying();
        //                commonState.player.WasPlaying = true;
        //            }
        //        }
        //        Librarian_lvGroups.SelectedItem = commonState.currentTone.Group;
        //    }
        //}

        private void Librarian_FilterPresetAndUser_Clicked(object sender, EventArgs e)
        {
            switch (toneNamesFilter)
            {
                case ToneNamesFilter.INIT:
                    QueryUserTones();
                    break;
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
                    Librarian_filterPresetAndUser.Text = "Preset and user tones";
                    break;
            }
            PopulateToneNames(Librarian_lvCategories.SelectedItem.ToString());
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

        private void Librarian_Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (initDone)
            {
                //t.Trace("private void tbSearch_TextChanged (" + "object" + sender + ", " + "TextChangedEventArgs" + e + ", " + ")");
                if (!String.IsNullOrEmpty(Librarian_tbSearch.Editor.Text) && Librarian_tbSearch.Editor.Text.Length > 2)
                {
                    Librarian_lvGroups.IsEnabled = false;
                    Librarian_lvCategories.IsEnabled = false;
                    usingSearchResults = true;
                    Librarian_PopulateSearchResults();
                }
                else
                {
                    Librarian_lvGroups.IsEnabled = true;
                    Librarian_lvCategories.IsEnabled = true;
                    usingSearchResults = false;
                    PopulateToneNames(commonState.currentTone.Category);
                }
            }
        }

        private void Librarian_btnMinus12keys_Clicked(object sender, EventArgs e)
        {
            commonState.midi.NoteOff(0, 64);
        }

        private void Librarian_btnPlus12keys_Clicked(object sender, EventArgs e)
        {
            commonState.midi.NoteOn(0, 64, 64);
        }

        private void Librarian_BtnEditTone_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowToneEditorPage();
        }

        private void Librarian_btnEditStudioSet_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowStudioSetEditorPage();
        }

        private void Librarian_btnResetVolume_Clicked(object sender, EventArgs e)
        {
        }

        private void Librarian_btnMotionalSurround_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowMotionalSurroundPage();
        }

        private void Librarian_btnFavorites_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowFavoritesPage(FavoriteAction.SHOW);
        }

        private void Librarian_btnAddFavorite_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowFavoritesPage(FavoriteAction.ADD);
        }

        private void Librarian_btnRemoveFavorite_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowFavoritesPage(FavoriteAction.REMOVE);
        }

        private void Librarian_btnResetHangingNotes_Clicked(object sender, EventArgs e)
        {
        }

        private void Librarian_btnPlay_Clicked(object sender, EventArgs e)
        {
            QueryUserTones();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Librarian functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private Boolean IsInitTone(byte[] data)
        {
            //t.Trace("private Boolean IsInitTone (" + "byte[]" + data + ", " + ")");
            char[] init = "INIT TONE   ".ToCharArray();
            Boolean initTone = true;
            for (byte i = 0; i < 12; i++)
            {
                if (init[i] != data[i + 11])
                {
                    initTone = false;
                    break;
                }
            }
            return initTone;
        }

        private Boolean IsInitKit(byte[] data)
        {
            //t.Trace("private Boolean IsInitKit (" + "byte[]" + data + ", " + ")");
            char[] init = "INIT KIT    ".ToCharArray();
            Boolean initTone = true;
            for (byte i = 0; i < 12; i++)
            {
                if (init[i] != data[i + 11])
                {
                    initTone = false;
                    break;
                }
            }
            return initTone;
        }

        /// <summary>
        /// Queries I-7 for user tones to add to the voicelist
        /// </summary>
        private async void QueryUserTones()
        {
            // Start with PCM Synth Tone, MainPage_MidiInPort_MessageReceived and Timer_Tick will handle the rest:
            Boolean response = await mainPage.DisplayAlert("INTEGRA_7 Librarian", "Do you want the librarian to scan your INTEGRA-7 for user tones, or will you only use the INTEGRA-7 preset tones?\r\n\r\n" +
                "Note: Scanning will change Tone on your INTEGRA-7, part 16.", "Yes", "No");
            //MessageDialog warning = new MessageDialog("Do you want the librarian to scan your INTEGRA-7 for user tones, or will you only use the INTEGRA-7 preset tones?\r\n\r\n" +
            //    "Note: Scanning will change Tone on your INTEGRA-7, part 16. If you have unsaved data, save before tapping or clicking \'Scan...\'!\r\n\r\n" +
            //    "This could take a while, so please select scanning option below:");
            //warning.Title = "Welcome!";
            //warning.Commands.Add(new UICommand { Label = "Scan all user tone slots", Id = 0 });
            //warning.Commands.Add(new UICommand { Label = "Scan until 10 empty slots found in row", Id = 1 });
            //warning.Commands.Add(new UICommand { Label = "Do not scan for user tones", Id = 2 });
            //var response = await warning.ShowAsync();
            //if ((Int32)response.Id == 0)
            //{
            //}
            if (response)
            {
                toneNamesFilter = ToneNamesFilter.ALL;
                Librarian_filterPresetAndUser.Text = "Preset and user tones";
                scanAll = await mainPage.DisplayAlert("INTEGRA_7 Librarian", "This could take a while, so please select scanning option below:", "Scan all user tone slots", "Scan only until 10 empty slots are found in row");
                msb = 87;
                lsb = 0;
                pc = 1;
                emptySlots = 10;
                scanning = true;
                userToneNumbers = new ushort[128];
                for (byte i = 0; i < 128; i++)
                {
                    userToneNumbers[i] = 0;
                }
                emptySlots = 0;
                QueryUserPCMSyntTones();
            }
            //else
            //{
            //    Init2();
            //}
        }

        // These 5 functions will change program on channel 16 and query to get the name.
        private void QueryUserPCMSyntTones()
        {
            //t.Trace("private void QueryUserPCMSyntTones()");
            commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x1c, 0x60, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0c };
            byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.PCM_SYNTH_TONE_COMMON;
            commonState.midi.SendSystemExclusive(message);
        }
        private void QueryUserPCMDrumKitTones()
        {
            //t.Trace("private void QueryUserPCMDrumKitTones()");
            commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x1c, 0x70, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0c };
            byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.PCM_DRUM_KIT_COMMON;
            commonState.midi.SendSystemExclusive(message);
        }
        private void QueryPcmDrumKitKeyName(byte Key)
        {
            //t.Trace("private void QueryPcmDrumKitKeyName()");
            byte[] address = new byte[] { 0x1c, 0x70, 0x10, 0x00 };
            address = hex2Midi.AddBytes128(address, new byte[] { 0x00, 0x00, Key, 0x00 });
            address = hex2Midi.AddBytes128(address, new byte[] { 0x00, 0x00, Key, 0x00 });
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0c };
            byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.PCM_KEY_NAME;
            commonState.midi.SendSystemExclusive(message);
        }
        private void QueryUserSuperNaturalAcousticTones()
        {
            //t.Trace("private void QueryUserSuperNaturalAcousticTones()");
            commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x1c, 0x62, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x46 };
            byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.SUPERNATURAL_ACOUSTIC_TONE_COMMON;
            commonState.midi.SendSystemExclusive(message);
        }
        private void QueryUserSuperNaturalSynthTones()
        {
            //t.Trace("private void QueryUserSuperNaturalSynthTones()");
            commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x1c, 0x61, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x40 };
            byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.SUPERNATURAL_SYNTH_TONE_COMMON;
            commonState.midi.SendSystemExclusive(message);
        }
        private void QueryUserSuperNaturalDrumKitTones()
        {
            //t.Trace("private void QueryUserSuperNaturalDrumKitTones()");
            commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x1c, 0x63, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0e };
            byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.SUPERNATURAL_DRUM_KIT_COMMON;
            commonState.midi.SendSystemExclusive(message);
        }
        private void QuerySnDrumKitKeyName(byte Key)
        {
            //t.Trace("private void QuerySnDrumKitKeyName()");
            byte[] address = new byte[] { 0x1c, 0x63, 0x10, 0x00 };
            address = hex2Midi.AddBytes128(address, new byte[] { 0x00, 0x00, Key, 0x00 });
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x04 };
            byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.SND_KEY_NAME;
            commonState.midi.SendSystemExclusive(message);
        }
        private void QuerySelectedStudioSet()
        {
            //t.Trace("private void QuerySelectedStudioSet()");
            commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x01, 0x00, 0x00, 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x07 };
            byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.CURRENT_SELECTED_STUDIO_SET;
            commonState.midi.SendSystemExclusive(message);
        }

        private void QuerySelectedTone()
        {
            //t.Trace("private void QuerySelectedTone()");
            commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
            byte[] address = new byte[] { 0x18, 0x00, (byte)(0x20 + commonState.CurrentPart), 0x00 };
            byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x09 };
            byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
            queryType = QueryType.CURRENT_SELECTED_TONE;
            commonState.midi.SendSystemExclusive(message);
        }

        public void ShowLibrarianPage()
        {
            page = _page.LIBRARIAN;
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
            //t.Trace("private void PopulateCategories (" + "String" + group + ", " + ")");
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
            //PopulateToneNames(Librarian_lvCategories.SelectedItem.ToString());
        }

        private void PopulateToneNames(String category)
        {
            //t.Trace("private void PopulateToneNames (" + "String" + category + ", " + ")");
            if (initDone || !scanning)
            {
                try
                {
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
                    String group = Librarian_lvGroups.SelectedItem.ToString();
                    foreach (List<String> tone in commonState.toneList.Tones.OrderBy(o => o[3]))
                    {
                        if (tone[0] == group && tone[1] == category
                            && (toneNamesFilter == ToneNamesFilter.USER && tone[8] == "(User)"
                            || toneNamesFilter == ToneNamesFilter.PRESET && tone[8] != "(User)"
                            || toneNamesFilter == ToneNamesFilter.INIT
                            || toneNamesFilter == ToneNamesFilter.ALL))
                        {
                            Librarian_ToneNames.Add(tone[3]);
                        }
                    }
                    if (commonState.currentTone == null)
                    {
                        List<String> toneData = new List<String>();
                        toneData.Add(Librarian_lvGroups.SelectedItem.ToString());
                        toneData.Add(Librarian_lvCategories.SelectedItem.ToString());
                        toneData.Add("");
                        toneData.Add(Librarian_ToneNames[0].ToString());
                        toneData.Add("");
                        toneData.Add("");
                        toneData.Add("");
                        toneData.Add("");
                        toneData.Add("");
                        toneData.Add("-1");
                        commonState.currentTone = new Tone(toneData);
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
                    else if (Librarian_ToneNames.Count > 0)
                    {
                        Librarian_lvToneNames.SelectedItem = Librarian_ToneNames[0];
                    }
                }
                catch (Exception e)
                {

                }
                //SetFavorite();
                Librarian_lvToneNames.ItemsSource = Librarian_ToneNames;
            }
        }

        private void Librarian_PopulateSearchResults()
        {
            try
            {
                //SearchResultSource.Clear();
                Librarian_ToneNames.Clear();
            }
            catch { }
            Librarian_ToneNames.Add("=== Tones =============");
            String searchString = Librarian_tbSearch.Editor.Text.ToLower();
            // Search voices:
            foreach (List<String> tone in commonState.toneList.Tones)
            {
                if (tone[3].ToLower().Contains(searchString)
                    && (toneNamesFilter == ToneNamesFilter.USER && tone[8] == "(User)"
                    || toneNamesFilter == ToneNamesFilter.PRESET && tone[8] != "(User)"
                    || toneNamesFilter == ToneNamesFilter.INIT
                    || toneNamesFilter == ToneNamesFilter.ALL))
                {
                    Librarian_ToneNames.Add(tone[3] + ", " + tone[0] + ", " + tone[1]);
                }
            }
            // Search drum sounds:
            Librarian_ToneNames.Add("=== Drums ==============");
            Boolean first = true; // To skip the key number column
            foreach (List<String> toneNames in commonState.drumKeyAssignLists.ToneNames)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    int skip = 2; // To skip the Group and Category names
                    foreach (String toneName in toneNames)
                    {
                        if (skip > 0)
                        {
                            skip--;
                        }
                        else
                        {
                            if (toneName.ToLower().Contains(searchString))
                            {
                                Librarian_ToneNames.Add(toneName + ", " + toneNames[0] + ", " + toneNames[1] + "\t");
                            }
                        }
                    }
                }
            }
            Librarian_lvToneNames.ItemsSource = Librarian_ToneNames;
        }

        private void PopulateToneData(Int32 Index)
        {
            //t.Trace("private void PopulateToneData (" + "Int32" + Index + ", " + ")");
            if (Index > -1)
            {
                List<String> tone = commonState.toneList.Tones[Index];
                Librarian_ltToneName.Text = tone[3];
                Librarian_ltType.Text = tone[8];
                Librarian_ltToneNumber.Text = tone[2];
                Librarian_ltPatchNumber.Text = tone[6];
                Librarian_ltPatchMSB.Text = tone[4];
                Librarian_ltPatchLSB.Text = tone[5];
                Librarian_ltProgramNumber.Text = tone[7];
                commonState.midi.ProgramChange(commonState.midi.GetMidiOutPortChannel(), tone[4], tone[5], tone[7]);
                if (IsFavorite())
                {
                    Librarian_btnShowFavorites.BackgroundColor = colorSettings.IsFavorite;
                }
                else
                {
                    Librarian_btnShowFavorites.BackgroundColor = colorSettings.Background;
                }
                Librarian_btnAddFavorite.IsEnabled = true;
            }
        }

        private void UpdateDrumNames()
        {
            //t.Trace("private void UpdateDrumNames()");
            //ClearKeyNames();
            if (commonState.currentTone.Category == "Drum" && commonState.drumKeyAssignLists.KeyboardNameList(commonState.currentTone.Group, commonState.currentTone.Name) != null)
            {
                commonState.keyNames = new List<String>();
                foreach (String keyName in commonState.drumKeyAssignLists.KeyboardNameList(commonState.currentTone.Group, commonState.currentTone.Name))
                {
                    commonState.keyNames.Add(keyName);
                }

                if (commonState.keyNames != null && commonState.keyNames.Count() > 0)
                {
                    for (Int32 i = 0; i < 61; i++)
                    {
                        if (i + lowKey - 22 > -1 && i + lowKey - 22 < commonState.keyNames.Count())
                        {
                            //SetKeyText(i, commonState.keyNames[i + lowKey - 22]);
                        }
                    }
                }
            }
        }
        private Boolean IsFavorite()
        {
            //t.Trace("private Boolean IsFavorite()");
            if (commonState.favoritesList != null)
            {
                foreach (FavoritesFolder folder in commonState.favoritesList.folders)
                {
                    foreach (Tone favorite in folder.FavoritesTones)
                    {
                        if (favorite.Group == commonState.currentTone.Group
                            && favorite.Category == commonState.currentTone.Category
                            && favorite.Name == commonState.currentTone.Name)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //private void ReadFavorites()
        //{
        //    try
        //    {
        //        if (localSettings.Values.ContainsKey("Favorites"))
        //        {
        //            if (commonState.favoritesList == null)
        //            {
        //                commonState.favoritesList = new FavoritesList();
        //                commonState.favoritesList.folders = new List<FavoritesFolder>();
        //            }
        //            commonState.favoritesList.folders.Clear();
        //            String foldersWithFavorites = ((String)localSettings.Values["Favorites"]).Trim('\n');
        //            // Format: [Folder name\v[Group index\tCategory index\tTone index\tGroup\tCategory\tTone\b]\f...]...
        //            // I.e. Split all by \f to get all folders with content.
        //            // Split each folder by \v to get folder name and all favorites together.
        //            // Split favorites by \b to get all favorites one by one.
        //            // Split each favorite by \t to get the 6 parts (3 indexes, 3 names).
        //            FavoritesFolder folder = null;
        //            foreach (String foldersWithFavoritePart in foldersWithFavorites.Split('\f'))
        //            {
        //                String[] folderWithFavorites = foldersWithFavoritePart.Split('\v');
        //                // Folder name:
        //                folder = new FavoritesFolder(folderWithFavorites[0]);
        //                commonState.favoritesList.folders.Add(folder);
        //                if (folderWithFavorites.Length > 1)
        //                {
        //                    String[] favoritesList = folderWithFavorites[1].Split('\b');
        //                    foreach (String favorite in favoritesList)
        //                    {
        //                        String[] favoriteParts = favorite.Split('\t');
        //                        try
        //                        {
        //                            if (favoriteParts.Length == 6)
        //                            {
        //                                // Add strings for Group, Category and Tone name:
        //                                // commonState.favoritesList.folders.Add(new String[] { favoriteParts[3], favoriteParts[4], favoriteParts[5] });
        //                                folder.FavoritesTones.Add(new Tone(Int32.Parse(favoriteParts[0]), Int32.Parse(favoriteParts[1]),
        //                                    Int32.Parse(favoriteParts[2]), favoriteParts[3], favoriteParts[4], favoriteParts[5]));
        //                            }
        //                        }
        //                        catch { }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}

        //private Note NoteFromMousePosition(Double x, Double y)
        //{
        //    Note key = new Note();
        //    key.NoteNumber = 255;
        //    key.Velocity = 255;

        //    Double keyBoardWidth = keyboard.ActualWidth;

        //    Double keyWidthAll = keyboard.ActualWidth / 61.7;
        //    Double keyWidthWhite = keyboard.ActualWidth / 36;
        //    Int32 keyNumber = 0;
        //    if (y > (0.56 * keyboard.ActualHeight))
        //    {
        //        // White key area
        //        keyNumber = (byte)((double)x / keyWidthWhite);
        //        if (keyNumber > 35)
        //        {
        //            keyNumber = 35;
        //        }
        //        key.NoteNumber = (byte)(whiteKeys[keyNumber] + transpose);
        //        if (currentNote > 127)
        //        {
        //            currentNote = 127;
        //        }
        //        key.Velocity = (byte)(127 * (y - (0.56 * keyboard.ActualHeight)) / (0.44 * keyboard.ActualHeight));
        //        if (key.Velocity > 127)
        //        {
        //            key.Velocity = 127;
        //        }
        //    }
        //    else
        //    {
        //        // All keys area
        //        keyNumber = (byte)((double)x / keyWidthAll);
        //        if (keyNumber > 60)
        //        {
        //            keyNumber = 60;
        //        }
        //        key.NoteNumber = (byte)(keyNumber + 36 + transpose);
        //        if (currentNote > 127)
        //        {
        //            currentNote = 127;
        //        }
        //        key.Velocity = (byte)(127 * (y / (0.56 * keyboard.ActualHeight)));
        //        if (key.Velocity > 127)
        //        {
        //            key.Velocity = 127;
        //        }
        //    }
        //    return key;
        //}

        //private void ClearKeyNames()
        //{
        //    //t.Trace("private void ClearKeyNames()");
        //    for (Int32 key = 0; key < 61; key++)
        //    {
        //        SetKeyText(key, "");
        //    }
        //}

        //private void SetKeyText(Int32 Key, String Text)
        //{
        //    //t.Trace("private void SetKeyText (" + "Int32" + Key + ", " + "String" + Text + ", " + ")");
        //    switch (Key)
        //    {
        //        case 0:
        //            K000.Text = Text;
        //            break;
        //        case 1:
        //            K001.Text = Text;
        //            break;
        //        case 2:
        //            K002.Text = Text;
        //            break;
        //        case 3:
        //            K003.Text = Text;
        //            break;
        //        case 4:
        //            K004.Text = Text;
        //            break;
        //        case 5:
        //            K005.Text = Text;
        //            break;
        //        case 6:
        //            K006.Text = Text;
        //            break;
        //        case 7:
        //            K007.Text = Text;
        //            break;
        //        case 8:
        //            K008.Text = Text;
        //            break;
        //        case 9:
        //            K009.Text = Text;
        //            break;
        //        case 10:
        //            K010.Text = Text;
        //            break;
        //        case 11:
        //            K011.Text = Text;
        //            break;
        //        case 12:
        //            K012.Text = Text;
        //            break;
        //        case 13:
        //            K013.Text = Text;
        //            break;
        //        case 14:
        //            K014.Text = Text;
        //            break;
        //        case 15:
        //            K015.Text = Text;
        //            break;
        //        case 16:
        //            K016.Text = Text;
        //            break;
        //        case 17:
        //            K017.Text = Text;
        //            break;
        //        case 18:
        //            K018.Text = Text;
        //            break;
        //        case 19:
        //            K019.Text = Text;
        //            break;
        //        case 20:
        //            K020.Text = Text;
        //            break;
        //        case 21:
        //            K021.Text = Text;
        //            break;
        //        case 22:
        //            K022.Text = Text;
        //            break;
        //        case 23:
        //            K023.Text = Text;
        //            break;
        //        case 24:
        //            K024.Text = Text;
        //            break;
        //        case 25:
        //            K025.Text = Text;
        //            break;
        //        case 26:
        //            K026.Text = Text;
        //            break;
        //        case 27:
        //            K027.Text = Text;
        //            break;
        //        case 28:
        //            K028.Text = Text;
        //            break;
        //        case 29:
        //            K029.Text = Text;
        //            break;
        //        case 30:
        //            K030.Text = Text;
        //            break;
        //        case 31:
        //            K031.Text = Text;
        //            break;
        //        case 32:
        //            K032.Text = Text;
        //            break;
        //        case 33:
        //            K033.Text = Text;
        //            break;
        //        case 34:
        //            K034.Text = Text;
        //            break;
        //        case 35:
        //            K035.Text = Text;
        //            break;
        //        case 36:
        //            K036.Text = Text;
        //            break;
        //        case 37:
        //            K037.Text = Text;
        //            break;
        //        case 38:
        //            K038.Text = Text;
        //            break;
        //        case 39:
        //            K039.Text = Text;
        //            break;
        //        case 40:
        //            K040.Text = Text;
        //            break;
        //        case 41:
        //            K041.Text = Text;
        //            break;
        //        case 42:
        //            K042.Text = Text;
        //            break;
        //        case 43:
        //            K043.Text = Text;
        //            break;
        //        case 44:
        //            K044.Text = Text;
        //            break;
        //        case 45:
        //            K045.Text = Text;
        //            break;
        //        case 46:
        //            K046.Text = Text;
        //            break;
        //        case 47:
        //            K047.Text = Text;
        //            break;
        //        case 48:
        //            K048.Text = Text;
        //            break;
        //        case 49:
        //            K049.Text = Text;
        //            break;
        //        case 50:
        //            K050.Text = Text;
        //            break;
        //        case 51:
        //            K051.Text = Text;
        //            break;
        //        case 52:
        //            K052.Text = Text;
        //            break;
        //        case 53:
        //            K053.Text = Text;
        //            break;
        //        case 54:
        //            K054.Text = Text;
        //            break;
        //        case 55:
        //            K055.Text = Text;
        //            break;
        //        case 56:
        //            K056.Text = Text;
        //            break;
        //        case 57:
        //            K057.Text = Text;
        //            break;
        //        case 58:
        //            K058.Text = Text;
        //            break;
        //        case 59:
        //            K059.Text = Text;
        //            break;
        //        case 60:
        //            K060.Text = Text;
        //            break;
        //    }
        //}

        //private void PlayNote(byte note, Int32 length)
        //{
        //    //t.Trace("private void PlayNote (" + "byte" + note + ", " + "Int32" + length + ", " + ")");
        //    commonState.midi.NoteOn(commonState.CurrentPart, note, 92);
        //    Task.Delay(length).Wait();
        //    commonState.midi.NoteOff(commonState.CurrentPart, note);
        //}

        //private void SetFavorite()
        //{
        //    //t.Trace("private void SetFavorite()");
        //    btnAddFavorite.IsEnabled = true;
        //    btnRemoveFavorite.IsEnabled = true;
        //}
    }
}
