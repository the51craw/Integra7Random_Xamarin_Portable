using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using INTEGRA_7_Xamarin.UWP;
using Xamarin.Forms;
using INTEGRA_7_Xamarin;

[assembly: Xamarin.Forms.Dependency(typeof(GenericHandlerInterface))]

[assembly: Dependency(typeof(MIDI))]

namespace INTEGRA_7_Xamarin.UWP
{
    public class GenericHandlerInterface: IGenericHandler
    {
        //enum QueryType
        //{
        //    NONE,
        //    CHECKING_I_7_READINESS,
        //    PCM_SYNTH_TONE_COMMON,
        //    PCM_SYNTH_TONE_COMMON2,
        //    PCM_DRUM_KIT_COMMON,
        //    SUPERNATURAL_ACOUSTIC_TONE_COMMON,
        //    SUPERNATURAL_SYNTH_TONE_COMMON,
        //    SUPERNATURAL_DRUM_KIT_COMMON,
        //    PCM_KEY_NAME,
        //    SND_KEY_NAME,
        //    CURRENT_SELECTED_STUDIO_SET,
        //    CURRENT_SELECTED_TONE,
        //}

        //Boolean scanAll = false;
        //UInt16 emptySlots = 10;

        ////ApplicationDataContainer localSettings = null; // use Application.Current.Properties instead!
        //CommonState commonState = null;
        //public List<List<String>> Lists = null;
        //private Boolean AutoUpdateChildLists = true;
        //private Int32 currentGroupIndex = -1;
        //private Int32 currentCategoryIndex = -1;
        //private Int32 currentToneNameIndex = -1;
        //private byte currentNote = 255; // > 127 when note is off
        //public static String[] lines;
        //private Int32 lowKey = 36;
        //private Int32 transpose = 0;
        //private byte[] notes = { 36, 40, 43, 48 };
        //private byte[] drumNotes = { 36, 38, 42, 45, 43, 41,
        //    60, 61, 56, 69, 70, 54, 65, 76, 77,
        //    46, 44, 51, 53, 52, 67, 68, 69, 70 };
        //private byte[] whiteKeys = { 36, 38, 40, 41, 43, 45,
        //    47, 48, 50, 52, 53, 55, 57, 59, 60, 62, 64, 65,
        //    67, 69, 71, 72, 74, 76, 77, 79, 81, 83, 84, 86,
        //    88, 89, 91, 93, 95, 96 };
        //private Boolean initDone = false;
        //private Boolean scanning = false;
        //private DispatcherTimer timer = null;
        //private Boolean followOutputPort = false;
        //private Boolean followingOutputPort = false;
        //private Boolean waitingForInputPortChange = false;
        //private String outputPortId;
        //private Int32 savedInputPort = -1;
        //private Int32 testingInputPortId;
        //private byte msb;
        //private byte lsb;
        //private byte pc;
        //private byte key;
        //String toneName;
        //String category;
        //byte toneCategory;
        //Int32 userToneIndex;
        //Boolean integra_7Ready = false;
        //byte integra_7ReadyCounter = 100;
        //SolidColorBrush green = null;
        //SolidColorBrush gray = null;
        //Boolean initMidi = false;
        //UInt16[] userToneNumbers;
        //QueryType queryType;
        //Boolean updateToneNames = false;
        //ToneCategories toneCategories = new ToneCategories();
        //Hex2Midi hex2Midi = new Hex2Midi();
        //SuperNATURALDrumKitInstrumentList superNATURALDrumKitInstrumentList = new SuperNATURALDrumKitInstrumentList();

        public MainPage mainPage { get; set; }
		
		

        public void GenericHandler(object sender, object e)
        {
            if (mainPage.midi.midiOutPort == null)
            {
                mainPage.midi.Init("INTEGRA-7");
            }
            //mainPage.midi.ProgramChange(0, 88, 0, 1);
        }
    }

    public sealed partial class MainPage
    {
        // For accessing INTEGRA_7_Xamarin.MainPage from UWP:
        private INTEGRA_7_Xamarin.MainPage mainPage;
        //public Int32 appType = 5;
        // Invisible comboboxes used by MIDI class (will always have INTEGRA-7 selected):
        private Picker OutputSelector;
        private Picker InputSelector;
        public MIDI midi;
        // For accessing the genericHandlerInterface:
        GenericHandlerInterface genericHandlerInterface;
        public Windows.UI.Core.CoreDispatcher dispatcher;
        //public MainPage mainPage;

        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new INTEGRA_7_Xamarin.App());
            Init();
        }
        
        private void Init()
        {
            // Get dispatcher:
            dispatcher = Dispatcher;
            // Get INTEGRA_7_Xamarin.MainPage:
            mainPage = INTEGRA_7_Xamarin.MainPage.GetMainPage();
            UIHandler.appType = UIHandler._appType.UWP;
            // Get the generic handler (same way as done in INTEGRA_7_Xamarin.UIHandler):
            genericHandlerInterface = (INTEGRA_7_Xamarin.UWP.GenericHandlerInterface)DependencyService.Get<IGenericHandler>();
            // Let genericHandlerInterface know this MainPage:
            genericHandlerInterface.mainPage = this;
            // Draw UI (function is in mainPage.uIHandler):
            mainPage.uIHandler.DrawPage();

            // We need invisible ComboBoxes to hold settings from the
            // corresponding Pickers in the Xamarin code.
            OutputSelector = mainPage.uIHandler.midiOutputDevice;
            InputSelector = mainPage.uIHandler.midiInputDevice;
            midi = new MIDI(mainPage, this, OutputSelector, InputSelector, 0, 0);
            midi.Init("INTEGRA-7");
        }

        private void Btn0_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
