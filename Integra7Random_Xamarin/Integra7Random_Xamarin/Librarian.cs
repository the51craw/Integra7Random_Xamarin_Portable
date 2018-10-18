using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Integra7Random_Xamarin
{
    public class Module
    {
        public String Name { get; set; }
        public byte SlotCount { get; set; }
        public byte BankMSB { get; set; }
        public byte BankLSB { get; set; }

        public Module(String Name, byte SlotCount, byte BankMSB, byte BankLSB)
        {
            this.Name = Name;
            this.SlotCount = SlotCount;
            this.BankMSB = BankMSB;
            this.BankLSB = BankLSB;
        }
    }

    public partial class UIHandler
    {
        enum ToneNamesFilter
        {
            ALL = 0,
            PRESET = 1,
            USER = 2,
            INIT = 3,
        }

        enum CurrentMidiRequest
        {
            NONE,
            GET_CURRENT_STUDIO_SET_NUMBER,
            GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN,
            STUDIO_SET_TITLES,
            STUDIO_SET_COMMON,
            SYSTEM_COMMON,
            STUDIO_SET_CHORUS,
            STUDIO_SET_CHORUS_OFF,
            STUDIO_SET_CHORUS_CHORUS,
            STUDIO_SET_CHORUS_DELAY,
            STUDIO_SET_CHORUS_GM2_CHORUS,
            STUDIO_SET_REVERB,
            STUDIO_SET_REVERB_OFF,
            STUDIO_SET_REVERB_ROOM1,
            STUDIO_SET_REVERB_ROOM2,
            STUDIO_SET_REVERB_HALL1,
            STUDIO_SET_REVERB_HALL2,
            STUDIO_SET_REVERB_PLATE,
            STUDIO_SET_REVERB_GM2_REVERB,
            STUDIO_SET_MOTIONAL_SURROUND,
            STUDIO_SET_MASTER_EQ,
            STUDIO_SET_PART,
            STUDIO_SET_PART_TONE_NAME,
            STUDIO_SET_PART_MIDI_PHASELOCK,
            STUDIO_SET_PART_EQ,
        }

        enum State
        {
            INIT,
            INIT_DONE,
            NONE,
            QUERYING_CURRENT_STUDIO_SET_NUMBER,
            QUERYING_STUDIO_SET_NAMES,
            QUERYING_SYSTEM_COMMON,
            QUERYING_STUDIO_SET_COMMON,
            QUERYING_STUDIO_SET_CHORUS,
            QUERYING_STUDIO_SET_CHORUS_OFF,
            QUERYING_STUDIO_SET_CHORUS_CHORUS,
            QUERYING_STUDIO_SET_CHORUS_DELAY,
            QUERYING_STUDIO_SET_CHORUS_GM2_CHORUS,
            QUERYING_STUDIO_SET_REVERB,
            QUERYING_STUDIO_SET_REVERB_OFF,
            QUERYING_STUDIO_SET_REVERB_ROOM1,
            QUERYING_STUDIO_SET_REVERB_ROOM2,
            QUERYING_STUDIO_SET_REVERB_HALL1,
            QUERYING_STUDIO_SET_REVERB_HALL2,
            QUERYING_STUDIO_SET_REVERB_PLATE,
            QUERYING_STUDIO_SET_REVERB_GM2_REVERB,
            QUERYING_STUDIO_SET_MOTIONAL_SURROUND,
            QUERYING_STUDIO_SET_MASTER_EQ,
            QUERYING_STUDIO_SET_PART,
            QUERYING_STUDIO_SET_PART_TONE_NAME,
            QUERYING_STUDIO_SET_PART_MIDI_PHASELOCK,
            QUERYING_STUDIO_SET_PART_EQ,
            DONE,
        }

        CurrentMidiRequest currentMidiRequest = CurrentMidiRequest.NONE;
        State state;
        Int32 timeoutCounter = 0;
        byte[] currentStudioSetNumberAsBytes = new byte[3];
        byte[] studioSetNumberIndexAsBytes = new byte[3];
        ProgressBar Librarian_progressBar;
        DrumKeyAssignLists drumKeyAssignLists = new DrumKeyAssignLists();
        List<String> DrumKeyList;
        byte[] whiteKeyNumbers = new byte[] { 36, 35, 33, 31, 30, 28, 26, 24, 23, 21, 19, 17, 16, 14, 12, 11, 9, 7, 5, 4, 2, 0 };
        byte[] blackKeyNumbers = new byte[] { 34, 32, 29, 27, 25, 22, 20, 18, 15, 13, 10, 8, 6, 3, 1 };
        Random random = new Random();

        // Librarian controls:
        public Picker Librarian_midiOutputDevice { get; set; }
        public Picker Librarian_midiInputDevice { get; set; }
        public Picker Librarian_midiOutputChannel { get; set; }
        public Picker Librarian_midiInputChannel { get; set; }

        Grid Librarian_MainGrid;
        Grid WaitWhileScanning;
        Grid Librarian_gridGroups;
        Button Librarian_lblGroups;
        Grid Librarian_gridGroupControls;
        Grid Librarian_gridTones;
        Button Librarian_lblTones;
        Grid Librarian_gridRandomButtons;
        ListView Librarian_lvToneNames;
        Grid Librarian_gridNameAndSlot;
        Grid Librarian_gridSaveDelete;
        Grid Librarian_gridKeyboard;
        Button Librarian_btnRandomAll;
        Button[] Librarian_btnRandom;
        LabeledTextInput Librarian_txtName;
        LabeledPicker Librarian_pickerSlot;
        Button Librarian_btnSave;
        Button Librarian_btnDelete;
        Button Librarian_btnPlay;
        Button[] Librarian_btnPlayOnChannel;
        Button Librarian_btnPlus12keys;
        Button Librarian_btnMinus12keys;
        MyLabel Librarian_lblKeys;
        Int32 headingHeight;
        private Int32 lowKey = 36;
        private Int32 waitingForI7 = 100;
        Int32 currentKey = -1;

        // Buttons for the keyboard:
        Button[] Librarian_btnWhiteKeys;
        Button[] Librarian_btnBlackKeys;

        CheckBox Librarian_cbExPCM;
        CheckBox Librarian_cbExSN1;
        CheckBox Librarian_cbExSN2;
        CheckBox Librarian_cbExSN3;
        CheckBox Librarian_cbExSN4;
        CheckBox Librarian_cbExSN5;
        CheckBox Librarian_cbExSN6;
        CheckBox Librarian_cbGM2_Drum_Kit;
        CheckBox Librarian_cbGM2_Tone;
        CheckBox Librarian_cbPCM_Drum_Kit;
        CheckBox Librarian_cbPCM_Synth_Tone;
        CheckBox Librarian_cbSRX_01;
        CheckBox Librarian_cbSRX_02;
        CheckBox Librarian_cbSRX_03;
        CheckBox Librarian_cbSRX_04;
        CheckBox Librarian_cbSRX_05;
        CheckBox Librarian_cbSRX_06;
        CheckBox Librarian_cbSRX_07;
        CheckBox Librarian_cbSRX_08;
        CheckBox Librarian_cbSRX_09;
        CheckBox Librarian_cbSRX_10;
        CheckBox Librarian_cbSRX_11;
        CheckBox Librarian_cbSRX_12;
        CheckBox Librarian_cbSuperNATURAL_Acoustic_Tone;
        CheckBox Librarian_cbSuperNATURAL_Drum_Kit;
        CheckBox Librarian_cbSuperNATURAL_Synth_Tone;

        SuperNATURALDrumKitInstrumentList superNATURALDrumKitInstrumentList = new SuperNATURALDrumKitInstrumentList();

        public UIHandler()
        {

        }

        public void DrawLibrarianPage()
        {
            //x = -1;
            //y = -1;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Librarian 
            // New suggested layout since we cannot write vertical text on keyboard:
            // |--------------------------------------------------------------------|
            // | lblGroups            | filterPresetAndUser  | Keyboard             |
            // |----------------------|----------------------|                      |
            // | gridTones            | gridRandomButtons    |                      |
            // |                      |----------------------|                      |
            // |                      |      Random all      |                      |
            // |                      |----------------------|                      |
            // |                      |    Random part 01    |                      |
            // |                      |----------------------|                      |
            // |                      |    Random part 02    |                      |
            // |                      |----------------------|                      |
            // |                      |        ...           |                      |
            // |                      |                      |                      |
            // |                      |                      |                      |
            // |                      |                      |                      |
            // |                      |                      |                      |
            // |                      |                      |                      |
            // |                      |                      |                      |
            // |                      |                      |                      |
            // |                      |                      |                      |
            // |                      |                      |                      |
            // |                      |                      |                      |
            // |                      |                      |                      |
            // |                      |----------------------|                      |
            // |                      |      Play/Stop       |                      |
            // |                      |----------------------|                      |
            // |                      | keyrange |+12k | -12k|                      |
            // |--------------------------------------------------------------------|

            // Create all controls ------------------------------------------------------------------------

            // Make a listview lvGroups for column 0:
            Librarian_lblGroups = new Button();
            Librarian_lblGroups.Text = "Synth types & Expansion slots to include";
            Librarian_lblGroups.IsEnabled = false;
            Librarian_gridGroupControls = new Grid();

            // Make checkboxes for the 26 group types:
            Librarian_cbExPCM = new CheckBox("ExPCM: HQ GM2 + HQ PCM Sound Col");
            Librarian_cbExSN1 = new CheckBox("ExSN1: Ethnic");
            Librarian_cbExSN2 = new CheckBox("ExSN2: Wood Winds");
            Librarian_cbExSN3 = new CheckBox("ExSN3: Session");
            Librarian_cbExSN4 = new CheckBox("ExSN4: A.Guitar");
            Librarian_cbExSN5 = new CheckBox("ExSN5: Brass");
            Librarian_cbExSN6 = new CheckBox("ExSN6: SFX");
            Librarian_cbGM2_Drum_Kit = new CheckBox("GM2 Drum Kit (PCM Drum Kit)");
            Librarian_cbGM2_Tone = new CheckBox("GM2 Tone (PCM Synth Tone)");
            Librarian_cbPCM_Drum_Kit = new CheckBox("PCM Drum Kit", true);
            Librarian_cbPCM_Synth_Tone = new CheckBox("PCM Synth Tone", true);
            Librarian_cbSRX_01 = new CheckBox("SRX-01: Dynamic Drum Kits");
            Librarian_cbSRX_02 = new CheckBox("SRX-02: Concert Piano");
            Librarian_cbSRX_03 = new CheckBox("SRX-03: Studio SRX");
            Librarian_cbSRX_04 = new CheckBox("SRX-04: Symphonique Strings");
            Librarian_cbSRX_05 = new CheckBox("SRX-05: Supreme Dance");
            Librarian_cbSRX_06 = new CheckBox("SRX-06: Complete Orchestra");
            Librarian_cbSRX_07 = new CheckBox("SRX-07: Ultimate Keys");
            Librarian_cbSRX_08 = new CheckBox("SRX-08: Platinum Trax");
            Librarian_cbSRX_09 = new CheckBox("SRX-09: World Collection");
            Librarian_cbSRX_10 = new CheckBox("SRX-10: Big Brass Ensemble");
            Librarian_cbSRX_11 = new CheckBox("SRX-11: Complete Piano");
            Librarian_cbSRX_12 = new CheckBox("SRX-12: Classic EPs");
            Librarian_cbSuperNATURAL_Acoustic_Tone = new CheckBox("SuperNATURAL Acoustic Tone", true);
            Librarian_cbSuperNATURAL_Synth_Tone = new CheckBox("SuperNATURAL Synth Tone", true);
            Librarian_cbSuperNATURAL_Drum_Kit = new CheckBox("SuperNATURAL Drum Kit", true);

            Librarian_lblTones = new Button();
            Librarian_lblTones.Text = "Random selection";
            Librarian_lblTones.IsEnabled = false;

            // Make Grids for column 0 - 1:
            Librarian_gridGroups = new Grid();
            Librarian_gridTones = new Grid();
            Librarian_gridRandomButtons = new Grid();

            // Make listviews lvToneNames and lvSearchResult for column 2:
            Librarian_lvToneNames = new ListView();
            Librarian_lvToneNames.BackgroundColor = colorSettings.Background;

            // Make a Grid for column 2:
            Librarian_gridKeyboard = new Grid();

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

            // Make the keyboard grid (We cannot use GridRow here due to limitation of byte
            // and we do not need to run a lot of setting we will change here anyway):
            Librarian_gridKeyboard.ColumnDefinitions = new ColumnDefinitionCollection();
            ColumnDefinition cdLength = new ColumnDefinition(); // Spanning first black key size,
            ColumnDefinition cdMargin = new ColumnDefinition(); // Spanning both to get white key size
            cdLength.Width = new GridLength(2, GridUnitType.Star);
            cdMargin.Width = new GridLength(1, GridUnitType.Star);
            Librarian_gridKeyboard.ColumnDefinitions.Add(cdLength);
            Librarian_gridKeyboard.ColumnDefinitions.Add(cdMargin);

            Librarian_gridKeyboard.RowDefinitions = new RowDefinitionCollection();
            for (Int32 i = 0; i < 22 * 16; i++)
            {
                Librarian_gridKeyboard.RowDefinitions.Add(new RowDefinition());
            }

            // Make the white keyboard keys:
            Librarian_btnWhiteKeys = new Button[22];
            for (byte i = 0; i < 22; i++)
            {
                Librarian_btnWhiteKeys[i] = new Button();
                Grid grid = new Grid();
                Grid.SetRowSpan(grid, 16);
                Grid.SetRow(grid, i * 16);
                Grid.SetColumnSpan(grid, 2);
                Grid.SetColumn(grid, 0);
                grid.Children.Add(Librarian_btnWhiteKeys[i]);
                Librarian_gridKeyboard.Children.Add(grid);
                Librarian_btnWhiteKeys[i].BackgroundColor = Color.FloralWhite;
                Librarian_btnWhiteKeys[i].TextColor = Color.Black;
                Librarian_btnWhiteKeys[i].Text = "";
                Librarian_btnWhiteKeys[i].StyleId = i.ToString();
                if (appType == _appType.MacOS)
                {
                    Librarian_btnWhiteKeys[i].Clicked += Librarian_btnWhiteKey_Klick;
                }
                else
                {
                    Librarian_btnWhiteKeys[i].Pressed += Librarian_btnWhiteKey_Pressed;
                    Librarian_btnWhiteKeys[i].Released += Librarian_btnWhiteKey_Released;
                }
                Librarian_btnWhiteKeys[i].BorderWidth = 0;
                if (i == 0)
                {
                    Librarian_btnWhiteKeys[i].Margin = new Thickness(2);
                }
                else
                {
                    Librarian_btnWhiteKeys[i].Margin = new Thickness(2, 0, 2, 2);
                }
            }

            // Make the black keyboard keys:
            Librarian_btnBlackKeys = new Button[15];
            byte[] need6fillers = { 3, 5, 8, 10, 13 };
            byte numberOfFillers = 0;
            Int32 position = 0;
            for (byte i = 0; i < 15; i++)
            {
                if (i == 0)
                {
                    numberOfFillers = 27;
                }
                else if (need6fillers.Contains(i))
                {
                    numberOfFillers = 24;
                }
                else
                {
                    numberOfFillers = 8;
                }
                Librarian_btnBlackKeys[i] = new Button();
                position += numberOfFillers;
                Grid grid = new Grid();
                Grid.SetRowSpan(grid, 10);
                Grid.SetRow(grid, position);
                Grid.SetColumnSpan(grid, 1);
                Grid.SetColumn(grid, 0);
                grid.Children.Add(Librarian_btnBlackKeys[i]);
                Librarian_gridKeyboard.Children.Add(grid);
                position += 8;
                Librarian_btnBlackKeys[i].Text = "";
                Librarian_btnBlackKeys[i].StyleId = i.ToString();
                Librarian_btnBlackKeys[i].BackgroundColor = Color.Black;
                Librarian_btnBlackKeys[i].TextColor = Color.White;
                Librarian_btnBlackKeys[i].Margin = new Thickness(2, 0, 0, 0);
                if (appType == _appType.MacOS)
                {
                    Librarian_btnBlackKeys[i].Clicked += Librarian_btnBlackKey_Klick;
                }
                else
                {
                    Librarian_btnBlackKeys[i].Pressed += Librarian_btnBlackKey_Pressed;
                    Librarian_btnBlackKeys[i].Released += Librarian_btnBlackKey_Released;
                }
                Librarian_btnBlackKeys[i].BorderWidth = 0;
            }

            // Add the buttons:
            Librarian_btnRandomAll = new Button();
            Librarian_btnRandomAll.BorderColor = Color.Black;
            Librarian_btnRandomAll.BorderWidth = 1;
            Librarian_btnRandomAll.Text = "Random all parts";
            Librarian_btnRandomAll.BackgroundColor = colorSettings.Background;
            Librarian_btnRandomAll.Clicked += Librarian_btnRandomAll_Clicked;
            Librarian_btnRandom = new Button[16];
            for (byte i = 0; i < 16; i++)
            {
                Librarian_btnRandom[i] = new Button();
                Librarian_btnRandom[i].BorderColor = Color.Black;
                Librarian_btnRandom[i].BorderWidth = 1;
                Librarian_btnRandom[i].Text = "Random part " + (i + 1).ToString();
                Librarian_btnRandom[i].BackgroundColor = colorSettings.Background;
                Librarian_btnRandom[i].StyleId = i.ToString();
                Librarian_btnRandom[i].Clicked += Librarian_btnRandom_Clicked;
            }

            Librarian_gridNameAndSlot = new Grid();
            Librarian_gridSaveDelete = new Grid();
            Librarian_txtName = new LabeledTextInput("Name: ", new byte[] { 1, 2 });
            Librarian_pickerSlot = new LabeledPicker("User slot: ", null, new byte[] { 1, 2 });
            Librarian_btnSave = new Button();
            Librarian_btnSave.Text = "Save studio set";
            Librarian_btnSave.BorderColor = Color.Black;
            Librarian_btnSave.BorderWidth = 1;
            Librarian_btnDelete = new Button();
            Librarian_btnDelete.Text = "Delete studio set";
            Librarian_btnDelete.BorderColor = Color.Black;
            Librarian_btnDelete.BorderWidth = 1;
            Librarian_btnPlay = new Button();
            Librarian_btnPlay.BorderColor = Color.Black;
            Librarian_btnPlay.BorderWidth = 1;
            Librarian_btnPlay.IsEnabled = false;
            Librarian_btnPlayOnChannel = new Button[16];

            for (byte i = 0; i < 16; i++)
            {
                Librarian_btnPlayOnChannel[i] = new Button();
                Librarian_btnPlayOnChannel[i].BorderColor = Color.Black;
                Librarian_btnPlayOnChannel[i].BorderWidth = 1;
                Librarian_btnPlayOnChannel[i].StyleId = i.ToString();
                Librarian_btnPlayOnChannel[i].Text = "Part " + (i + 1).ToString();
                Librarian_btnPlayOnChannel[i].Clicked += Librarian_btnPlay_Clicked;
            }
            Librarian_btnPlus12keys = new Button();
            Librarian_btnPlus12keys.BorderColor = Color.Black;
            Librarian_btnPlus12keys.BorderWidth = 1;
            Librarian_btnMinus12keys = new Button();
            Librarian_btnMinus12keys.BorderColor = Color.Black;
            Librarian_btnMinus12keys.BorderWidth = 1;
            Librarian_lblKeys = new MyLabel();
            Librarian_btnPlay.Text = "Play/Stop";
            Librarian_btnPlus12keys.Text = "+12";
            Librarian_btnMinus12keys.Text = "-12";
            ShowKeyNumbering();

            Librarian_btnPlay.BackgroundColor = colorSettings.Background;
            Librarian_btnPlus12keys.BackgroundColor = colorSettings.Background;
            Librarian_btnMinus12keys.BackgroundColor = colorSettings.Background;
            Librarian_lblKeys.BackgroundColor = colorSettings.Background;
            Librarian_btnSave.BackgroundColor = colorSettings.Background;
            Librarian_btnDelete.BackgroundColor = colorSettings.Background;

            // Add handlers -------------------------------------------------------------------------------

            Librarian_btnSave.Clicked += Librarian_btnSave_Clicked;
            Librarian_btnDelete.Clicked += Librarian_btnDelete_Clicked;
            Librarian_btnPlus12keys.Clicked += Librarian_btnPlus12keys_Clicked;
            Librarian_btnMinus12keys.Clicked += Librarian_btnMinus12keys_Clicked;

            // Assemble grids with controls ---------------------------------------------------------------

            headingHeight = 30;
            if (appType == _appType.ANDROID)
            {
                headingHeight = 64;
            }

            // Assemble column 0:
            Librarian_gridGroups.RowDefinitions = new RowDefinitionCollection();
            Librarian_gridGroups.RowDefinitions.Add(new RowDefinition());
            Librarian_gridGroups.RowDefinitions.Add(new RowDefinition());
            Librarian_gridGroups.Children.Add((new GridRow(0, new View[] { Librarian_lblGroups }, null, false, false)).Row);
            Librarian_gridGroups.Children.Add((new GridRow(1, new View[] { Librarian_gridGroupControls }, null, false, false)).Row);
            Librarian_gridGroupControls.RowDefinitions = new RowDefinitionCollection();
            for (byte i = 0; i < 25; i++)
            {
                Librarian_gridGroupControls.RowDefinitions.Add(new RowDefinition());
            }
            Librarian_gridGroupControls.Children.Add((new GridRow(0, new View[] { Librarian_cbExSN1 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(1, new View[] { Librarian_cbExSN2 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(2, new View[] { Librarian_cbExSN3 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(3, new View[] { Librarian_cbExSN4 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(4, new View[] { Librarian_cbExSN5 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(5, new View[] { Librarian_cbExSN6 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(6, new View[] { Librarian_cbGM2_Drum_Kit }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(7, new View[] { Librarian_cbGM2_Tone }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(8, new View[] { Librarian_cbPCM_Drum_Kit }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(9, new View[] { Librarian_cbPCM_Synth_Tone }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(10, new View[] { Librarian_cbSRX_01 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(11, new View[] { Librarian_cbSRX_02 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(12, new View[] { Librarian_cbSRX_03 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(13, new View[] { Librarian_cbSRX_04 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(14, new View[] { Librarian_cbSRX_05 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(15, new View[] { Librarian_cbSRX_06 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(16, new View[] { Librarian_cbSRX_07 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(17, new View[] { Librarian_cbSRX_08 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(18, new View[] { Librarian_cbSRX_09 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(19, new View[] { Librarian_cbSRX_10 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(20, new View[] { Librarian_cbSRX_11 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(21, new View[] { Librarian_cbSRX_12 }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(22, new View[] { Librarian_cbSuperNATURAL_Acoustic_Tone }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(23, new View[] { Librarian_cbSuperNATURAL_Synth_Tone }, null, false, false)).Row);
            Librarian_gridGroupControls.Children.Add((new GridRow(24, new View[] { Librarian_cbSuperNATURAL_Drum_Kit }, null, false, false)).Row);
            Librarian_gridGroups.RowDefinitions[0].Height = new GridLength(headingHeight, GridUnitType.Absolute);
            Librarian_gridGroups.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);

            // Assemble column 1:
            Librarian_gridTones.RowDefinitions = new RowDefinitionCollection();
            Librarian_gridTones.RowDefinitions.Add(new RowDefinition());
            Librarian_gridTones.RowDefinitions.Add(new RowDefinition());
            Librarian_gridTones.Children.Add((new GridRow(0, new View[] { Librarian_lblTones }, null, false, false)).Row);
            Librarian_gridTones.Children.Add((new GridRow(1, new View[] { Librarian_gridRandomButtons }, null, false, false)).Row);
            Librarian_gridNameAndSlot.Children.Add((new GridRow(0, new View[] { Librarian_txtName, Librarian_pickerSlot }, null, false)).Row);
            Librarian_gridSaveDelete.Children.Add((new GridRow(0, new View[] { Librarian_btnSave, Librarian_btnDelete }, null, false)).Row);
            Librarian_gridRandomButtons.RowDefinitions = new RowDefinitionCollection();
            for (byte i = 0; i < 25; i++)
            {
                Librarian_gridRandomButtons.RowDefinitions.Add(new RowDefinition());
            }
            Librarian_gridRandomButtons.Children.Add((new GridRow(0, new View[] { Librarian_btnRandomAll }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(1, new View[] { Librarian_btnRandom[0] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(2, new View[] { Librarian_btnRandom[1] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(3, new View[] { Librarian_btnRandom[2] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(4, new View[] { Librarian_btnRandom[3] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(5, new View[] { Librarian_btnRandom[4] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(6, new View[] { Librarian_btnRandom[5] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(7, new View[] { Librarian_btnRandom[6] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(8, new View[] { Librarian_btnRandom[7] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(9, new View[] { Librarian_btnRandom[8] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(10, new View[] { Librarian_btnRandom[9] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(11, new View[] { Librarian_btnRandom[10] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(12, new View[] { Librarian_btnRandom[11] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(13, new View[] { Librarian_btnRandom[12] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(14, new View[] { Librarian_btnRandom[13] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(15, new View[] { Librarian_btnRandom[14] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(16, new View[] { Librarian_btnRandom[15] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(17, new View[] { Librarian_btnPlay }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(18, new View[] { Librarian_btnPlayOnChannel[0], Librarian_btnPlayOnChannel[1], Librarian_btnPlayOnChannel[2], Librarian_btnPlayOnChannel[3] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(19, new View[] { Librarian_btnPlayOnChannel[4], Librarian_btnPlayOnChannel[5], Librarian_btnPlayOnChannel[6], Librarian_btnPlayOnChannel[7] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(20, new View[] { Librarian_btnPlayOnChannel[8], Librarian_btnPlayOnChannel[9], Librarian_btnPlayOnChannel[10], Librarian_btnPlayOnChannel[11] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(21, new View[] { Librarian_btnPlayOnChannel[12], Librarian_btnPlayOnChannel[13], Librarian_btnPlayOnChannel[14], Librarian_btnPlayOnChannel[15] }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(22, new View[] { Librarian_lblKeys, Librarian_btnMinus12keys, Librarian_btnPlus12keys }, new byte[] { 1, 1, 1 }, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(23, new View[] { Librarian_gridNameAndSlot }, null, false)).Row);
            Librarian_gridRandomButtons.Children.Add((new GridRow(24, new View[] { Librarian_gridSaveDelete }, null, false)).Row);
            Librarian_gridTones.RowDefinitions[0].Height = new GridLength(headingHeight, GridUnitType.Absolute);
            Librarian_gridTones.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);

            // Assemble LibrarianStackLayout --------------------------------------------------------------

            LibrarianStackLayout = new StackLayout();
            Librarian_MainGrid = new Grid();
            Librarian_MainGrid.Children.Add((new GridRow(0, new View[] { Librarian_gridGroups, Librarian_gridTones, Librarian_gridKeyboard }, new byte[] { 5, 7, 3 })).Row);
            // Make the entire grid background black to show as borders around controls by using margins:
            Librarian_MainGrid.BackgroundColor = Color.Black;
            LibrarianStackLayout.Children.Add((new GridRow(0, new View[] { Librarian_MainGrid })).Row);
            WaitWhileScanning = new Grid();
            Librarian_progressBar = new ProgressBar();
            Label lblWait = new Label();
            lblWait.Text = "Please wait while reading Studio Set from your INTEGRA-7...";
            WaitWhileScanning.Children.Add((new GridRow(0, new View[] { Librarian_progressBar })).Row);
            WaitWhileScanning.Children.Add((new GridRow(1, new View[] { lblWait })).Row);
            LibrarianStackLayout.Children.Add((new GridRow(0, new View[] { WaitWhileScanning })).Row);
            WaitWhileScanning.IsVisible = true;
            Librarian_MainGrid.IsVisible = false;

            // Prepare to obtain a list of studio set names when midi is initiated:
            state = State.INIT;
            Device.StartTimer(TimeSpan.FromMilliseconds(1), Timer_Tick);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Librarian handlers
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private Boolean Timer_Tick()
        {
            if (commonState == null || commonState.midi == null || ! commonState.midi.MidiIsReady())
            {
                waitingForI7--;
                if (waitingForI7 == 0)
                {
                    QuitWithMessage("Your INTEGRA-7 is not responding.");
                }
                return true;
            }
            else if (state == State.INIT)
            {
                // Get a list of all studio set names. Start by storing the current studio set number.
                // Note that consequent queries will be sent from MidiInPort_MessageReceived and Timer_Tick.
                commonState.studioSetNames = new List<String>();
                initDone = true;
                scanning = false;
                QueryCurrentStudioSetNumber(true); // And scan all studio set names.
            }
            else if (currentMidiRequest == CurrentMidiRequest.STUDIO_SET_TITLES && state != State.DONE)
            {
                // Update progress bar
                Librarian_progressBar.Progress = (double)studioSetNumberIndexAsBytes[2] / (double)63;
            }
            else if (currentMidiRequest == CurrentMidiRequest.STUDIO_SET_TITLES && state == State.DONE)
            {
                // Populate studio set selector and swap view
                currentMidiRequest = CurrentMidiRequest.NONE;
                foreach (String name in commonState.studioSetNames)
                {
                    Librarian_pickerSlot.Picker.Items.Add(name);
                }
                Librarian_pickerSlot.Picker.SelectedIndex = currentStudioSetNumberAsBytes[2];
                WaitWhileScanning.IsVisible = false;
                Librarian_MainGrid.IsVisible = true;
            }
            return true;
        }

        private async void QuitWithMessage(String message)
        {
            String response = await mainPage.DisplayActionSheet(message, null, null, new String[] { "Close app" });
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public void Librarian_MidiInPort_MessageReceived()
        {
            //t.Trace("private void MainPage_MidiInPort_MessageReceived");
            if (queryType == QueryType.CHECKING_I_7_READINESS)
            {
                integra_7Ready = true;
            }
            else if (initDone)
            {
                try
                {
                    if (scanning == false)
                    {
                        // Get and store the current studio set number
                        if (rawData[0] == 0xf0)
                        {
                            if ((currentMidiRequest == CurrentMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER
                                || currentMidiRequest == CurrentMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN)
                                && state == State.QUERYING_CURRENT_STUDIO_SET_NUMBER)
                            {
                                // We have asked for the current studio set number in order to restore it when we have iterated all names:
                                currentStudioSetNumberAsBytes[0] = 0x55;
                                currentStudioSetNumberAsBytes[1] = 0x00;
                                currentStudioSetNumberAsBytes[2] = rawData[13];

                                // Also store in commonState:
                                commonState.CurrentStudioSet = currentStudioSetNumberAsBytes[2];

                                if (currentMidiRequest == CurrentMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN)
                                {
                                    // Start querying all studio sets:
                                    currentMidiRequest = CurrentMidiRequest.STUDIO_SET_TITLES;
                                    state = State.QUERYING_STUDIO_SET_NAMES;
                                    studioSetNumberIndexAsBytes[0] = 0x55;
                                    studioSetNumberIndexAsBytes[1] = 0x00;
                                    studioSetNumberIndexAsBytes[2] = 0x00;
                                    ScanForStudioSetNames();
                                }
                            }
                            else if (currentMidiRequest == CurrentMidiRequest.STUDIO_SET_TITLES && rawData[0] == 0xf0)
                            {
                                // We have asked for the first/next studio set:
                                String text = "";
                                for (Int32 i = 0x0b; i < rawData.Length - 2; i++)
                                {
                                    text += (char)rawData[i];
                                }
                                commonState.studioSetNames.Add(text);
                                studioSetNumberIndexAsBytes[2]++;

                                // Query next studio set if this was not last one:
                                if (studioSetNumberIndexAsBytes[2] < 64)
                                {
                                    // Ask for it:
                                    ScanForStudioSetNames(); // Answer will be caught here.
                                }
                                else
                                {
                                    // All titles received, set a status that will be caught in Timer_Tick:
                                    state = State.DONE;
                                }
                            }
                            else
                            {
                                // Tell Timer_Tick that System Common has been read:
                                state = State.DONE; // This will be caught in Timer_Tick
                            }
                        }
                    }
                }
                catch { }
            }
        }

        private async void Librarian_btnSave_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Librarian_txtName.Editor.Text))
            {
                await mainPage.DisplayActionSheet("You must type a name for the studio set!", null, null, new string[] { "Cancel" });
            }
            else if (Librarian_txtName.Editor.Text.StartsWith("INIT STUDIO"))
            {
                await mainPage.DisplayActionSheet("You can not use INIT STUDIO as name!", null, null, new string[] { "Cancel" });
            }
            else
            {
                Boolean write = true;
                if (!((String)Librarian_pickerSlot.Picker.SelectedItem).StartsWith("INIT STUDIO"))
                {
                    String response = await mainPage.DisplayActionSheet("This slot contains another Studio Set. Are you sure you want to overwrite it?", null, null, new String[] { "Yes", "No" });
                    if (response == "No")
                    {
                        write = false;
                    }
                }
                if (write)
                {
                    // Store the new Studio Set name:
                    String name = Librarian_txtName.Editor.Text;
                    while (name.Length < 16)
                    {
                        name = name + " ";
                    }
                    byte[] address = { 0x18, 0x00, 0x00, 0x00 };
                    byte[] data = new UTF8Encoding().GetBytes(name);
                    byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, data);
                    timeoutCounter = 500;
                    commonState.midi.SendSystemExclusive(bytes);
                    // Save all partials:

                    // Make INTEGRA-7 save the Studio Set:
                    address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                    data = new byte[] { 0x55, 0x00, (byte)(Librarian_pickerSlot.Picker.SelectedIndex), 0x7f };
                    bytes = commonState.midi.SystemExclusiveRQ1Message(address, data);
                    timeoutCounter = 500;
                    commonState.midi.SendSystemExclusive(bytes);

                    // Add the new studio set name to the studio set selector:
                    Librarian_pickerSlot.Picker.Items[Librarian_pickerSlot.Picker.SelectedIndex] = Librarian_txtName.Editor.Text;
                    Librarian_pickerSlot.Picker.SelectedIndex = studioSetNumberIndexAsBytes[2];
                }
            }
        }

        private async void Librarian_btnDelete_Clicked(object sender, EventArgs e)
        {
            //t.Trace("private void btnStudioSetDelete_Click (" + "object" + sender + ", " + "RoutedEventArgs" + e + ", " + ")");
            if (!Librarian_txtName.Editor.Text.StartsWith("INIT STUDIO"))
            {
                Boolean write = false;
                if (!((String)Librarian_pickerSlot.Picker.SelectedItem).StartsWith("INIT STUDIO"))
                {
                    String response = await mainPage.DisplayActionSheet("Are you sure you want to delete this Studio Set?", null, null, new String[] { "Yes", "No" });
                    if (response == "Yes")
                    {
                        write = true;
                    }
                    //App.Current.Quit();
                }
                if (write)
                {
                    // Change the name:
                    byte[] address = { 0x18, 0x00, 0x00, 0x00 };
                    byte[] data = Encoding.UTF8.GetBytes("INIT STUDIO     ");
                    byte[] bytes = commonState.midi.SystemExclusiveDT1Message(address, data);
                    timeoutCounter = 500;
                    commonState.midi.SendSystemExclusive(bytes);
                    // InitializeComponent the studio set:
                    address = new byte[] { 0x0f, 0x00, (byte)(Librarian_pickerSlot.Picker.SelectedIndex), 0x00 };
                    data = new byte[] { 0x7f, 0x7f, 0x00, 0x00 };
                    bytes = commonState.midi.SystemExclusiveRQ1Message(address, data);
                    timeoutCounter = 500;
                    commonState.midi.SendSystemExclusive(bytes);
                    // Save the studio set:
                    address = new byte[] { 0x0f, 0x00, 0x10, 0x00 };
                    data = new byte[] { 0x55, 0x00, (byte)(Librarian_pickerSlot.Picker.SelectedIndex), 0x7f };
                    bytes = commonState.midi.SystemExclusiveRQ1Message(address, data);
                    timeoutCounter = 500;
                    commonState.midi.SendSystemExclusive(bytes);

                    // Remove the studio set name from the studio set selector:
                    Librarian_pickerSlot.Picker.Items[Librarian_pickerSlot.Picker.SelectedIndex] = "INIT STUDIO";
                    Librarian_pickerSlot.Picker.SelectedIndex = studioSetNumberIndexAsBytes[2];

                    // Now, get the freshly initiated part:
                    //QueryStudioSetPart();
                }
            }
        }

        private void Librarian_btnMinus12keys_Clicked(object sender, EventArgs e)
        {
            if (lowKey >= 12)
            {
                lowKey -= 12;
                ShowKeyNumbering();
                UpdateDrumNames();
            }
        }

        private void Librarian_btnPlus12keys_Clicked(object sender, EventArgs e)
        {
            if (lowKey < 92)
            {
                lowKey += 12;
                ShowKeyNumbering();
                UpdateDrumNames();
            }
        }

        private void ShowKeyNumbering()
        {
            if (lowKey + 3 * 12 > 127)
            {
                Librarian_lblKeys.Text = "Keys " + (lowKey + 1).ToString() + " - 128";
                Librarian_btnWhiteKeys[0].IsVisible = false;
                Librarian_btnWhiteKeys[1].IsVisible = false;
                Librarian_btnWhiteKeys[2].IsVisible = false;
                Librarian_btnWhiteKeys[3].IsVisible = false;
                Librarian_btnBlackKeys[0].IsVisible = false;
                Librarian_btnBlackKeys[1].IsVisible = false;
            }
            else
            {
                Librarian_lblKeys.Text = "Keys " + (lowKey + 1).ToString() + " - " + (lowKey + 37).ToString();
                Librarian_btnWhiteKeys[0].IsVisible = true;
                Librarian_btnWhiteKeys[1].IsVisible = true;
                Librarian_btnWhiteKeys[2].IsVisible = true;
                Librarian_btnWhiteKeys[3].IsVisible = true;
                Librarian_btnBlackKeys[0].IsVisible = true;
                Librarian_btnBlackKeys[1].IsVisible = true;
            }
        }

        private void Librarian_btnRandomAll_Clicked(object sender, EventArgs e)
        {
            for (byte part = 0; part < 16; part++)
            {
                Librarian_Random(part, Librarian_btnRandom[part]);
            }
        }

        private void Librarian_btnRandom_Clicked(object sender, EventArgs e)
        {
            byte part = (byte)(Int32.Parse(((Button)sender).StyleId));
            commonState.CurrentPart = part;
            Librarian_Random(part, (Button)sender);

            if (part == 9)
            {
                UpdateDrumNames();
            }
            else
            {
                ClearKeyNames();
            }
        }

        private void Librarian_btnPlay_Clicked(object sender, EventArgs e)
        {
            // Buttons StyleId are numbered 0 - 15.
            // Turning on a part is number 1 - 16.
            // Turning of any part is 0. (But they queue up if not always turned off between turning on!)
            byte part = (byte)(Int32.Parse(((Button)sender).StyleId));
            if (((Button)sender).Text.StartsWith("*"))
            {
                byte[] address = new byte[] { 0x0f, 0x00, 0x20, 0x00 };
                byte[] data = new byte[] { 0x00 };
                byte[] package = commonState.midi.SystemExclusiveDT1Message(address, data);
                commonState.midi.SendSystemExclusive(package);
                for (byte i = 0; i < 16; i++)
                {
                    Librarian_btnPlayOnChannel[i].Text = "Part " + (i + 1).ToString();
                }
            }
            else
            {
                byte[] address = new byte[] { 0x0f, 0x00, 0x20, 0x00 };
                byte[] data = new byte[] { 0x00 };
                byte[] package = commonState.midi.SystemExclusiveDT1Message(address, data);
                commonState.midi.SendSystemExclusive(package);
                data = new byte[] { (byte)(Int32.Parse(((Button)sender).StyleId) + 1) };
                package = commonState.midi.SystemExclusiveDT1Message(address, data);
                commonState.midi.SendSystemExclusive(package);
                Librarian_midiOutputChannel.SelectedIndex = Int32.Parse(((Button)sender).StyleId);
                commonState.midi.SetMidiOutPortChannel(part);
                commonState.CurrentPart = part;
                for (byte i = 0; i < 16; i++)
                {
                    Librarian_btnPlayOnChannel[i].Text = "Part " + (i + 1).ToString();
                }
                ((Button)sender).Text = "*" + ((Button)sender).Text;
            }

            if (part == 9)
            {
                UpdateDrumNames();
            }
            else
            {
                ClearKeyNames();
            }
        }

        private void Librarian_btnWhiteKey_Klick(object sender, EventArgs e)
        {
            byte noteNumber = (byte)(whiteKeyNumbers[Int32.Parse(((Button)sender).StyleId)] + lowKey);
            if (noteNumber == currentNote)
            {
                commonState.midi.NoteOff(commonState.CurrentPart, currentNote);
                currentNote = 255;
            }
            else if (noteNumber < 128)
            {
                if (currentNote < 128)
                {
                    commonState.midi.NoteOff(commonState.CurrentPart, currentNote);
                }
                currentNote = noteNumber;
                commonState.midi.NoteOn(commonState.CurrentPart, noteNumber, 100);
            }
        }

        private void Librarian_btnBlackKey_Klick(object sender, EventArgs e)
        {
            byte noteNumber = (byte)(blackKeyNumbers[Int32.Parse(((Button)sender).StyleId)] + lowKey);
            if (noteNumber == currentNote)
            {
                commonState.midi.NoteOff(commonState.CurrentPart, currentNote);
                currentNote = 255;
            }
            else if (noteNumber < 128)
            {
                if (currentNote < 128)
                {
                    commonState.midi.NoteOff(commonState.CurrentPart, currentNote);
                }
                currentNote = noteNumber;
                commonState.midi.NoteOn(commonState.CurrentPart, noteNumber, 64);
            }
        }

        private void Librarian_btnWhiteKey_Pressed(object sender, EventArgs e)
        {
            byte noteNumber = (byte)(whiteKeyNumbers[Int32.Parse(((Button)sender).StyleId)] + lowKey);
            if (noteNumber != currentNote && currentNote < 128) // This will only happen if user has moved the pointer to another key without prior releasing it, and then press another key. Using this triggers supernatural legato behaviour.
            {
                commonState.midi.NoteOn(commonState.CurrentPart, noteNumber, 100);
                commonState.midi.NoteOff(commonState.CurrentPart, currentNote);
            }
            else
            {
                commonState.midi.NoteOn(commonState.CurrentPart, noteNumber, 100);
            }
            currentNote = noteNumber;
        }

        private void Librarian_btnWhiteKey_Released(object sender, EventArgs e)
        {
            byte noteNumber = (byte)(whiteKeyNumbers[Int32.Parse(((Button)sender).StyleId)] + lowKey);
            commonState.midi.NoteOff(commonState.CurrentPart, currentNote);
            currentNote = 255;
        }

        private void Librarian_btnBlackKey_Pressed(object sender, EventArgs e)
        {
            byte noteNumber = (byte)(blackKeyNumbers[Int32.Parse(((Button)sender).StyleId)] + lowKey);
            if (noteNumber != currentNote && currentNote < 128) // This will only happen if user has moved the pointer to another key without prior releasing it, and then press another key. Using this triggers supernatural legato behaviour.
            {
                commonState.midi.NoteOn(commonState.CurrentPart, noteNumber, 100);
                commonState.midi.NoteOff(commonState.CurrentPart, currentNote);
            }
            else
            {
                commonState.midi.NoteOn(commonState.CurrentPart, noteNumber, 100);
            }
            currentNote = noteNumber;
        }

        private void Librarian_btnBlackKey_Released(object sender, EventArgs e)
        {
            byte noteNumber = (byte)(blackKeyNumbers[Int32.Parse(((Button)sender).StyleId)] + lowKey);
            commonState.midi.NoteOff(commonState.CurrentPart, currentNote);
            currentNote = 255;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Librarian functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void QueryCurrentStudioSetNumber(Boolean scan = true)
        {
            //t.Trace("private void QueryCurrentStudioSetNumber (" + "Boolean" + scan + ", " + ")");
            // If this is the first time (scan = true)
            // We must iterate all studio sets on the INTEGRA-7 in order to get the names.
            // Get the currently set studio set in order to restore it when done iterating,
            // or, if this is not first time, to set selector correct:
            if (scan)
            {
                currentMidiRequest = CurrentMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER_AND_SCAN;
            }
            else
            {
                currentMidiRequest = CurrentMidiRequest.GET_CURRENT_STUDIO_SET_NUMBER;
            }
            state = State.QUERYING_CURRENT_STUDIO_SET_NUMBER;
            byte[] address = { 0x01, 0x00, 0x00, 0x04 };
            byte[] length = { 0x00, 0x00, 0x00, 0x03 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            timeoutCounter = 500;
            commonState.midi.SendSystemExclusive(bytes); // Answer will be caught in MidiInPort_MessageReceived.
        }

        public void ScanForStudioSetNames()
        {
            //t.Trace("private void ScanForStudioSetNames()");
            // Set studio set according to currentStudioSetNumber:
            SetStudioSet(studioSetNumberIndexAsBytes);

            // Request studio set name:
            byte[] address = { 0x18, 0x00, 0x00, 0x00 };
            byte[] length = { 0x00, 0x00, 0x00, 0x10 };
            byte[] bytes = commonState.midi.SystemExclusiveRQ1Message(address, length);
            timeoutCounter = 500;
            commonState.midi.SendSystemExclusive(bytes); // Answer will be caught in MidiInPort_MessageReceived.
            currentMidiRequest = CurrentMidiRequest.STUDIO_SET_TITLES;
        }

        private void SetStudioSet(byte[] number)
        {
            //t.Trace("private void SetStudioSet (" + "byte[]" + number + ", " + ")");
            commonState.midi.ProgramChange((byte)15, (byte)85, (byte)0, (byte)(number[2] + 1));
        }

        private void Librarian_Random(byte part, Button button)
        {
            List<Module> modules = null;
            if (part == 9)
            {
                modules = MakeDrumModulesDrumsList();
            }
            else
            {
                modules = MakeToneModulesToneList();
            }
            Int32 maxRandom = 0;
            foreach (Module module in modules)
            {
                maxRandom += module.SlotCount;
            }
            Int32 selection = random.Next(maxRandom);
            Int32 totalInstrumentCount = 0;
            foreach (Module module in modules)
            {
                totalInstrumentCount += module.SlotCount;
                if (selection < totalInstrumentCount)
                {
                    Int32 programChangeNumber = selection - (totalInstrumentCount - module.SlotCount);
                    Int32 toneInList = commonState.toneList.Get(module.BankMSB, module.BankLSB, (byte)programChangeNumber);
                    button.Text = "Part " + (part + 1).ToString() + ": " + module.Name + ", " + commonState.toneList.Tones[toneInList][1] + ", " + commonState.toneList.Tones[toneInList][3];
                    commonState.midi.ProgramChange(part, module.BankMSB, module.BankLSB, (byte)(programChangeNumber + 1));
                    if (part == 9)
                    {
                        DrumKeyList = drumKeyAssignLists.NameList(commonState.toneList.Tones[toneInList][0], commonState.toneList.Tones[toneInList][3]);
                    }
                    break;
                }
            }
        }

        /*
            Makes a list of Tone Module objects corresponding to the checked modules:
        */
        public List<Module> MakeToneModulesToneList()
        {
            List<Module> modules = new List<Module>();
            if (Librarian_cbExPCM.Switch.IsToggled)
            {
                modules.Add(new Module("ExPCM", 128, 97, 0));
                modules.Add(new Module("ExPCM", 128, 97, 1));
                modules.Add(new Module("ExPCM", 128, 97, 2));
            }
            if (Librarian_cbExSN1.Switch.IsToggled)
            {
                modules.Add(new Module("ExSN1", 17, 89, 96));
            }
            if (Librarian_cbExSN2.Switch.IsToggled)
            {
                modules.Add(new Module("ExSN2", 17, 89, 97));
            }
            if (Librarian_cbExSN3.Switch.IsToggled)
            {
                modules.Add(new Module("ExSN3", 50, 89, 98));
            }
            if (Librarian_cbExSN4.Switch.IsToggled)
            {
                modules.Add(new Module("ExSN4", 12, 89, 99));
            }
            if (Librarian_cbExSN5.Switch.IsToggled)
            {
                modules.Add(new Module("ExSN5", 12, 89, 100));
            }
            if (Librarian_cbGM2_Tone.Switch.IsToggled)
            {
                modules.Add(new Module("GM2 Tone", 128, 121, 0));
            }
            if (Librarian_cbPCM_Synth_Tone.Switch.IsToggled)
            {
                modules.Add(new Module("PCM Synth Tone", 128, 87, 64));
                modules.Add(new Module("PCM Synth Tone", 128, 87, 65));
                modules.Add(new Module("PCM Synth Tone", 128, 87, 66));
                modules.Add(new Module("PCM Synth Tone", 128, 87, 67));
                modules.Add(new Module("PCM Synth Tone", 128, 87, 68));
                modules.Add(new Module("PCM Synth Tone", 128, 87, 69));
                modules.Add(new Module("PCM Synth Tone", 128, 87, 70));
            }
            if (Librarian_cbSRX_01.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-01", 41, 93, 0));
            }
            if (Librarian_cbSRX_02.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-02", 50, 93, 1));
            }
            if (Librarian_cbSRX_03.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-03", 128, 93, 2));
            }
            if (Librarian_cbSRX_04.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-04", 128, 93, 3));
            }
            if (Librarian_cbSRX_05.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-05", 128, 93, 4));
                modules.Add(new Module("SRX-05", 128, 93, 5));
                modules.Add(new Module("SRX-05", 128, 93, 6));
            }
            if (Librarian_cbSRX_06.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-06", 128, 93, 7));
                modules.Add(new Module("SRX-06", 128, 93, 8));
                modules.Add(new Module("SRX-06", 128, 93, 9));
                modules.Add(new Module("SRX-06", 56, 93, 10));
            }
            if (Librarian_cbSRX_07.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-07", 128, 93, 11));
                modules.Add(new Module("SRX-07", 128, 93, 12));
                modules.Add(new Module("SRX-07", 128, 93, 13));
                modules.Add(new Module("SRX-07", 91, 93, 14));
            }
            if (Librarian_cbSRX_08.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-08", 128, 93, 15));
                modules.Add(new Module("SRX-08", 128, 93, 16));
                modules.Add(new Module("SRX-08", 128, 93, 17));
                modules.Add(new Module("SRX-08", 64, 93, 18));
            }
            if (Librarian_cbSRX_09.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-09", 128, 93, 19));
                modules.Add(new Module("SRX-09", 128, 93, 20));
                modules.Add(new Module("SRX-09", 128, 93, 21));
                modules.Add(new Module("SRX-09", 30, 93, 22));
            }
            if (Librarian_cbSRX_10.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-10", 100, 93, 23));
            }
            if (Librarian_cbSRX_11.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-11", 42, 93, 24));
            }
            if (Librarian_cbSRX_12.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-12", 50, 93, 26));
            }
            if (Librarian_cbSuperNATURAL_Acoustic_Tone.Switch.IsToggled)
            {
                modules.Add(new Module("SN-A", 128, 89, 64));
                modules.Add(new Module("SN-A", 128, 89, 65));
            }
            if (Librarian_cbSuperNATURAL_Synth_Tone.Switch.IsToggled)
            {
                modules.Add(new Module("SN-S", 128, 95, 64));
                modules.Add(new Module("SN-S", 128, 95, 65));
                modules.Add(new Module("SN-S", 128, 95, 66));
                modules.Add(new Module("SN-S", 128, 95, 67));
                modules.Add(new Module("SN-S", 128, 95, 68));
                modules.Add(new Module("SN-S", 128, 95, 69));
                modules.Add(new Module("SN-S", 128, 95, 70));
                modules.Add(new Module("SN-S", 128, 95, 71));
                modules.Add(new Module("SN-S", 85, 95, 72));
            }

            return modules;
        }


        /*
            Makes a list of Drum Module objects corresponding to the checked modules:
        */
        public List<Module> MakeDrumModulesDrumsList()
        {
            List<Module> modules = new List<Module>();
            if (Librarian_cbExPCM.Switch.IsToggled)
            {
                modules.Add(new Module("ExPCM", 19, 97, 3));
            }
            if (Librarian_cbExSN6.Switch.IsToggled)
            {
                modules.Add(new Module("ExSN6", 7, 88, 101));
            }
            if (Librarian_cbGM2_Drum_Kit.Switch.IsToggled)
            {
                modules.Add(new Module("GM2 Drum Kit", 120, 0, 9));
            }
            if (Librarian_cbPCM_Drum_Kit.Switch.IsToggled)
            {
                modules.Add(new Module("PCM Drum Kit", 14, 86, 64));
            }
            if (Librarian_cbSRX_01.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-01", 79, 92, 0));
            }
            if (Librarian_cbSRX_03.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-03", 12, 92, 2));
            }
            if (Librarian_cbSRX_05.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-05", 34, 92, 4));
            }
            if (Librarian_cbSRX_06.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-06", 34, 92, 7));
            }
            if (Librarian_cbSRX_07.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-07", 11, 92, 11));
            }
            if (Librarian_cbSRX_08.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-08", 21, 92, 15));
            }
            if (Librarian_cbSRX_09.Switch.IsToggled)
            {
                modules.Add(new Module("SRX-09", 12, 92, 19));
            }
            if (Librarian_cbSuperNATURAL_Drum_Kit.Switch.IsToggled)
            {
                modules.Add(new Module("SN-D", 26, 88, 64));
            }

            return modules;
        }
        //private Boolean IsInitTone(byte[] data)
        //{
        //    //t.Trace("private Boolean IsInitTone (" + "byte[]" + data + ", " + ")");
        //    char[] init = "INIT TONE   ".ToCharArray();
        //    Boolean initTone = true;
        //    for (byte i = 0; i < 12; i++)
        //    {
        //        if (init[i] != data[i + 11])
        //        {
        //            initTone = false;
        //            break;
        //        }
        //    }
        //    return initTone;
        //}

        //private Boolean IsInitKit(byte[] data)
        //{
        //    //t.Trace("private Boolean IsInitKit (" + "byte[]" + data + ", " + ")");
        //    char[] init = "INIT KIT    ".ToCharArray();
        //    Boolean initTone = true;
        //    for (byte i = 0; i < 12; i++)
        //    {
        //        if (init[i] != data[i + 11])
        //        {
        //            initTone = false;
        //            break;
        //        }
        //    }
        //    return initTone;
        //}

        //// These 5 functions will change program on channel 16 and query to get the name.
        //private void QueryUserPCMSyntTones()
        //{
        //    //t.Trace("private void QueryUserPCMSyntTones()");
        //    commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
        //    byte[] address = new byte[] { 0x1c, 0x60, 0x00, 0x00 };
        //    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0c };
        //    byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    queryType = QueryType.PCM_SYNTH_TONE_COMMON;
        //    commonState.midi.SendSystemExclusive(message);
        //}
        //private void QueryUserPCMDrumKitTones()
        //{
        //    //t.Trace("private void QueryUserPCMDrumKitTones()");
        //    commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
        //    byte[] address = new byte[] { 0x1c, 0x70, 0x00, 0x00 };
        //    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0c };
        //    byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    queryType = QueryType.PCM_DRUM_KIT_COMMON;
        //    commonState.midi.SendSystemExclusive(message);
        //}
        //private void QueryPcmDrumKitKeyName(byte Key)
        //{
        //    //t.Trace("private void QueryPcmDrumKitKeyName()");
        //    byte[] address = new byte[] { 0x1c, 0x70, 0x10, 0x00 };
        //    address = hex2Midi.AddBytes128(address, new byte[] { 0x00, 0x00, Key, 0x00 });
        //    address = hex2Midi.AddBytes128(address, new byte[] { 0x00, 0x00, Key, 0x00 });
        //    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0c };
        //    byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    queryType = QueryType.PCM_KEY_NAME;
        //    commonState.midi.SendSystemExclusive(message);
        //}

        //private void QueryUserSuperNaturalAcousticTones()
        //{
        //    //t.Trace("private void QueryUserSuperNaturalAcousticTones()");
        //    commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
        //    byte[] address = new byte[] { 0x1c, 0x62, 0x00, 0x00 };
        //    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x46 };
        //    byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    queryType = QueryType.SUPERNATURAL_ACOUSTIC_TONE_COMMON;
        //    commonState.midi.SendSystemExclusive(message);
        //}
        //private void QueryUserSuperNaturalSynthTones()
        //{
        //    //t.Trace("private void QueryUserSuperNaturalSynthTones()");
        //    commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
        //    byte[] address = new byte[] { 0x1c, 0x61, 0x00, 0x00 };
        //    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x40 };
        //    byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    queryType = QueryType.SUPERNATURAL_SYNTH_TONE_COMMON;
        //    commonState.midi.SendSystemExclusive(message);
        //}
        //private void QueryUserSuperNaturalDrumKitTones()
        //{
        //    //t.Trace("private void QueryUserSuperNaturalDrumKitTones()");
        //    commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
        //    byte[] address = new byte[] { 0x1c, 0x63, 0x00, 0x00 };
        //    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x0e };
        //    byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    queryType = QueryType.SUPERNATURAL_DRUM_KIT_COMMON;
        //    commonState.midi.SendSystemExclusive(message);
        //}
        //private void QuerySnDrumKitKeyName(byte Key)
        //{
        //    //t.Trace("private void QuerySnDrumKitKeyName()");
        //    byte[] address = new byte[] { 0x1c, 0x63, 0x10, 0x00 };
        //    address = hex2Midi.AddBytes128(address, new byte[] { 0x00, 0x00, Key, 0x00 });
        //    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x04 };
        //    byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    queryType = QueryType.SND_KEY_NAME;
        //    commonState.midi.SendSystemExclusive(message);
        //}
        //private void QuerySelectedStudioSet()
        //{
        //    //t.Trace("private void QuerySelectedStudioSet()");
        //    commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
        //    byte[] address = new byte[] { 0x01, 0x00, 0x00, 0x00 };
        //    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x07 };
        //    byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    queryType = QueryType.CURRENT_SELECTED_STUDIO_SET;
        //    commonState.midi.SendSystemExclusive(message);
        //}

        //private void QuerySelectedTone()
        //{
        //    //t.Trace("private void QuerySelectedTone()");
        //    commonState.midi.ProgramChange(0x0f, msb, lsb, pc);
        //    byte[] address = new byte[] { 0x18, 0x00, (byte)(0x20 + commonState.CurrentPart), 0x00 };
        //    byte[] length = new byte[] { 0x00, 0x00, 0x00, 0x09 };
        //    byte[] message = commonState.midi.SystemExclusiveRQ1Message(address, length);
        //    queryType = QueryType.CURRENT_SELECTED_TONE;
        //    commonState.midi.SendSystemExclusive(message);
        //}

        public void ShowLibrarianPage()
        {
            if (!LibrarianIsCreated)
            {
                DrawLibrarianPage();
                mainStackLayout.Children.Add(LibrarianStackLayout);
                LibrarianIsCreated = true;
            }
            page = _page.LIBRARIAN;
            LibrarianStackLayout.IsVisible = true;

            // Set font size:
            SetFontSizes(LibrarianStackLayout);
        }

        private void UpdateDrumNames()
        {
            //t.Trace("private void UpdateDrumNames()");
            ClearKeyNames();
            commonState.keyNames = new List<String>();
            //foreach (String keyName in commonState.drumKeyAssignLists.KeyboardNameList(commonState.currentTone.Group, commonState.currentTone.Name))
            try
            {
                foreach (String keyName in DrumKeyList)
                {
                    commonState.keyNames.Add(keyName);
                }

                if (commonState.keyNames != null && commonState.keyNames.Count() > 0)
                {
                    for (Int32 i = 0; i < 37; i++)
                    {
                        if (i + lowKey - 21 > -1 && i + lowKey - 21 < commonState.keyNames.Count())
                        {
                            SetKeyText(i, commonState.keyNames[i + lowKey - 21]);
                        }
                    }
                }
            } catch { }
        }

        private void ClearKeyNames()
        {
            //t.Trace("private void ClearKeyNames()");
            for (Int32 key = 0; key < 61; key++)
            {
                SetKeyText(key, "");
            }
        }

        private void SetKeyText(Int32 Key, String Text)
        {
            //t.Trace("private void SetKeyText (" + "Int32" + Key + ", " + "String" + Text + ", " + ")");
            Int32 tempKeyNumber; // Derived from tone and lowKey, but then transformed to indicate actual key button.
            // keyIndexes to find key buttons. If < 200 use as index in whiteKeys, else subtract 200 and use to index blackKeys
            Int32[] keyIndexes = new Int32[] { 21, 214, 20, 213, 19, 18, 212, 17, 211, 16, 210, 15, 14, 209, 13, 208, 12, 11, 207, 10, 206, 9, 205, 8, 7, 203, 6, 204, 5, 4, 202, 3, 201, 2, 200, 1, 0 };
            tempKeyNumber = Key; // - lowKey;
            if (tempKeyNumber < 37)
            {
                if (keyIndexes[tempKeyNumber] < 200)
                {
                    Librarian_btnWhiteKeys[keyIndexes[tempKeyNumber]].Text = Text;
                }
                else
                {
                    Librarian_btnBlackKeys[keyIndexes[tempKeyNumber] - 200].Text = Text;
                }
            }
        }
    }
}
