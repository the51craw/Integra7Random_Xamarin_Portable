using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using INTEGRA_7_Xamarin.Droid;
using Android.Content;
using Android.Hardware.Usb;
using System.Collections.Generic;
using System;

//[assembly: Xamarin.Forms.Dependency(typeof(GenericHandlerInterface))]
//[assembly: Dependency(typeof(Android_MIDI))]

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
    [IntentFilter(new[] { UsbManager.ActionUsbDeviceAttached })]
    //[MetaData(UsbManager.ActionUsbDeviceAttached, Resource = "@xml/device_filter")]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        private static String ACTION_USB_PERMISSION = "eu.mrmartin.MIDI.USB_PERMISSION";
        private static String USB_ENDPOINT_XFER_BULK = "android.hardware.usb.action.USB_ENDPOINT_XFER_BULK";
        //private static String USB_DEVICE_ATTACHED = "eu.mrmartin.MIDI.USB_DEVICE_ATTACHED";
        private static String USB_DEVICE_ATTACHED = "android.hardware.usb.action.USB_DEVICE_ATTACHED";
        public PendingIntent mPermissionIntent = null;
        public UsbManager usbManager = null;
        public static UsbInterface usbInterface = null;
        public UsbDevice usbDevice = null;
        public UsbEndpoint outputEndpoint = null;
        public UsbEndpoint inputEndpoint = null;
        public USB usb = null;
        //public Android_MIDI midi = null;

        //private BroadcastReceiver mUsbReceiver = new BroadcastReceiver()
        //{

        //    public void onReceive(Context context, Intent intent)
        //    {
        //        String action = intent.getAction();
        //        if (ACTION_USB_PERMISSION.equals(action))
        //        {
        //            synchronized(this) {
        //                UsbDevice device = (UsbDevice)intent.getParcelableExtra(UsbManager.EXTRA_DEVICE);

        //                if (intent.getBooleanExtra(UsbManager.EXTRA_PERMISSION_GRANTED, false))
        //                {
        //                    if (device != null)
        //                    {
        //                        //call method to set up device communication
        //                    }
        //                }
        //                else
        //                {
        //                    Log.d(TAG, "permission denied for device " + device);
        //                }
        //            }
        //        }
        //    }
        //};


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
            Xamarin.Forms.DependencyService.Register<IMidi>();

            //Int32 deviceIndex = 0;
            //Int32 deviceCount = 0;
            //Int32 interfaceIndex;
            //Int32 endpointIndex;
            //UsbDevice[] usbDevices;
            //UsbInterface[][] usbInterfaces = null;
            //UsbEndpoint[][][] usbEndpoints = null;
            //UsbInterface usbInterface = null;
            //UsbDevice usbDevice = null;
            //UsbEndpoint outputEndpoint = null;
            //UsbEndpoint inputEndpoint = null;
            //Int32 count = 0;

            // Get INTEGRA_7_Xamarin.MainPage:
            MainPage_Portable = INTEGRA_7_Xamarin.MainPage.GetMainPage();
            UIHandler.appType = UIHandler._appType.ANDROID;
            MainPage_Portable.uIHandler.DrawPages();
            OutputSelector = MainPage_Portable.uIHandler.Librarian_midiOutputDevice;
            InputSelector = MainPage_Portable.uIHandler.Librarian_midiInputDevice;
            MainPage_Portable.SetDeviceSpecificMainPage(this);

            UsbManager usbManager = (UsbManager)GetSystemService(Context.UsbService);
            usb = new USB(usbManager);

            if (usb.Device != null && usb.Interface != null && usb.OutputEndpoint != null && usb.InputEndpoint != null)
            {
                PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, new Intent(ACTION_USB_PERMISSION), 0);
                IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
                filter.AddAction(USB_ENDPOINT_XFER_BULK);
                filter.AddAction(USB_DEVICE_ATTACHED);
                //filter.AddAction(USB_DEVICE_ATTACHED);
                //filter.AddAction(Intent.ActionDefault);
                //filter.AddAction(Intent.Action);
                //filter.AddAction("eu.mrmartin.MIDI.VIEW");
                RegisterReceiver(usb, filter);
                //IntentFilter filter2 = new IntentFilter(USB_ENDPOINT_XFER_BULK);
                //RegisterReceiver(usb, filter2);

                usb.Manager.RequestPermission(usb.Device, pendingIntent);
                usb.HasPermission = usb.Manager.HasPermission(usb.Device);
            }

            MainPage_Portable.uIHandler.commonState.midi.Init(MainPage_Portable, "INTEGRA-7", OutputSelector, InputSelector, this, 0, 0);
            MainPage_Portable.platform_specific = new object[] { usb };
            MainPage_Portable.uIHandler.ShowLibrarianPage();
        }
    }
}

