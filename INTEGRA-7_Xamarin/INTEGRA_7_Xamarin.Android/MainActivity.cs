using System;

using Android;
using Android.App;
using Android.Content.PM;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
using Android.OS;
//using Android.Media.Midi;
//using INTEGRA_7_Xamarin.Droid;
using Xamarin.Forms;
//using Windows.UI.Core;
//using Android.Content;
//using Mono;
using INTEGRA_7_Xamarin.Droid;

//[assembly: Xamarin.Forms.Dependency(typeof(GenericHandlerInterface))]
[assembly: Dependency(typeof(MIDI))]

namespace INTEGRA_7_Xamarin.Droid
{
    //public class GenericHandlerInterface : IGenericHandler
    //{
    //    //public MainPage mainPage { get; set; }
    //    //public MidiManager midiManager;

    //    public void GenericHandler(object sender, object e)
    //    {
    //        //Mono.Runtime.
    //        //Android.Runtime.JNIEnv.CallObjectMethod(midiManager, )
    //        //PortInfo.GetName();
    //        //if (context.MidiService.getPackageManager().hasSystemFeature(PackageManager.FeatureMidi))
    //        //    //{
    //        //    //    // do MIDI stuff
    //        //    //}
    //        //    Android.Media.Midi.MidiManager.GetObject<MidiManager>()
    //        //MidiManager m = (MidiManager)Context.getSystemService(Context.MidiService);
    //        //if (mainPage.midi.midiOutPort == null)
    //        //{
    //        //    mainPage.midi.Init("INTEGRA-7");
    //        //}
    //        //mainPage.midi.ProgramChange(0, 88, 0, 1);
    //    }
    //}

    [Activity(Label = "INTEGRA_7_Xamarin", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //GenericHandlerInterface genericHandlerInterface;
        // For accessing INTEGRA_7_Xamarin.MainPage from UWP:
        private INTEGRA_7_Xamarin.MainPage MainPage_Portable;
        // Invisible comboboxes used by MIDI class (will always have INTEGRA-7 selected):
        private Picker OutputSelector;
        private Picker InputSelector;
        //public MIDI midi;
		

        protected override void OnCreate(Bundle bundle)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            UIHandler.appType = UIHandler._appType.ANDROID;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            Init();
        }

        private void Init()
        {
            //MidiManager midiManager = (MidiManager)GetSystemService(Context.MidiService);
            //MidiDeviceInfo[] midiDeviceInfo = midiManager.GetDevices();
            Xamarin.Forms.DependencyService.Register<IMidi>();
            // Get INTEGRA_7_Xamarin.MainPage:
            MainPage_Portable = INTEGRA_7_Xamarin.MainPage.GetMainPage();
            UIHandler.appType = UIHandler._appType.ANDROID;
            // Get mainActivity:
            //mainActivity = this;
            // Get the generic handler (same way as done in INTEGRA_7_Xamarin.UIHandler):
            //genericHandlerInterface = (INTEGRA_7_Xamarin.Droid.GenericHandlerInterface)DependencyService.Get<IGenericHandler>();
            // Let genericHandlerInterface know this MainPage:
            //genericHandlerInterface.mainPage = this;
            // Draw UI (function is in mainPage.uIHandler):
            MainPage_Portable.uIHandler.DrawPages();
            OutputSelector = MainPage_Portable.uIHandler.Librarian_midiOutputDevice;
            InputSelector = MainPage_Portable.uIHandler.Librarian_midiInputDevice;
            MainPage_Portable.SetDeviceSpecificMainPage(this);
            MainPage_Portable.uIHandler.commonState.midi.Init(MainPage_Portable, "INTEGRA-7", OutputSelector, InputSelector, null, 0, 0);
            MainPage_Portable.uIHandler.ShowLibrarianPage();
        }

        //public MainActivity GetMainActivity()
        //{
        //    return this;
        //}
    }
}

