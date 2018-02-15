using System;
using System.Collections.ObjectModel;
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
    public partial class UIHandler
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
        public System.Collections.Generic.List<System.Collections.Generic.List<String>> Lists = null;
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

        INTEGRA_7_Xamarin.MainPage mainPage;
        StackLayout mainStackLayout { get; set; }
        StackLayout LibrarianStackLayout;
        StackLayout EditorStackLayout;
        StackLayout FavoritesStackLayout;
        StackLayout StudioSetEditorStackLayout;
        StackLayout MotionalSurroundStackLayout;

        public static _appType appType;
        public static ColorSettings colorSettings { get; set; }
        public static BorderThicknesSettings borderThicknesSettings { get; set; }
        _page page;

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
            initDone = true;
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
            DrawLibrarianPage();
            DrawToneEditorPage();
            DrawMotionalSurroundPage();
            DrawStudioSetEditorPage();
            DrawFavoritesPage();

            commonState.midi = DependencyService.Get<IMidi>();
            commonState.midi.Init("INTEGRA-7", mainPage);

            // Always start by showing librarian:
            ShowLibrarianPage();
        }
    }
}
