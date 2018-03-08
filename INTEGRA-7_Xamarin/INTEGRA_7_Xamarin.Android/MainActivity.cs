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
    [IntentFilter(new[] { UsbManager.ActionUsbDeviceAttached })]
    //[MetaData(UsbManager.ActionUsbDeviceAttached, Resource = "@xml/device_filter")]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        private static String ACTION_USB_PERMISSION = "com.android.example.USB_PERMISSION";
        public PendingIntent mPermissionIntent = null;
        public UsbManager usbManager = null;
        public static UsbInterface usbInterface = null;
        public UsbDevice usbDevice = null;
        public UsbEndpoint outputEndpoint = null;
        public UsbEndpoint inputEndpoint = null;
        public USB usb = null;

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

            Int32 deviceIndex = 0;
            Int32 deviceCount = 0;
            Int32 interfaceIndex;
            Int32 endpointIndex;
            UsbDevice[] usbDevices;
            UsbInterface[][] usbInterfaces = null;
            UsbEndpoint[][][] usbEndpoints = null;
            UsbInterface usbInterface = null;
            UsbDevice usbDevice = null;
            UsbEndpoint outputEndpoint = null;
            UsbEndpoint inputEndpoint = null;
            Int32 count = 0;

            // Get INTEGRA_7_Xamarin.MainPage:
            MainPage_Portable = INTEGRA_7_Xamarin.MainPage.GetMainPage();
            UIHandler.appType = UIHandler._appType.ANDROID;
            MainPage_Portable.uIHandler.DrawPages();
            OutputSelector = MainPage_Portable.uIHandler.Librarian_midiOutputDevice;
            InputSelector = MainPage_Portable.uIHandler.Librarian_midiInputDevice;
            MainPage_Portable.SetDeviceSpecificMainPage(this);

            // Get the USB manager:
            UsbManager usbManager = (UsbManager)GetSystemService(Context.UsbService);
            UsbReceiver usbReceiver = new UsbReceiver();

            // Get the USB devices (normally one in an Android device):
            usbDevices = new UsbDevice[usbManager.DeviceList.Count];
            foreach (KeyValuePair<string, UsbDevice> keyValuePair in usbManager.DeviceList)
            {
                usbDevices[deviceIndex] = keyValuePair.Value;
                deviceIndex++;
            }
            deviceCount = deviceIndex;

            // Create usbInterface and usbEnpont lists:
            usbInterfaces = new UsbInterface[deviceCount][];
            usbEndpoints = new UsbEndpoint[deviceCount][][];

            // Loop all (one) devices and look for interfaces and endpoints:
            for (deviceIndex = 0; deviceIndex < deviceCount; deviceIndex++)
            {
                if (usbDevices[deviceIndex].InterfaceCount > 0)
                {
                    if (usbDevices[deviceIndex].ProductName == "INTEGRA-7")
                    {
                        usbDevice = usbDevices[deviceIndex];
                        usbInterfaces[deviceIndex] = new UsbInterface[usbDevices[deviceIndex].InterfaceCount];
                        for (interfaceIndex = 0; interfaceIndex < usbDevices[deviceIndex].InterfaceCount; interfaceIndex++)
                        {
                            usbInterfaces[deviceIndex][interfaceIndex] = usbDevices[deviceIndex].GetInterface(interfaceIndex);
                        }
                        usbEndpoints[deviceIndex] = new UsbEndpoint[usbDevices[deviceIndex].InterfaceCount][];
                        for (interfaceIndex = 0; interfaceIndex < usbDevices[deviceIndex].InterfaceCount; interfaceIndex++)
                        {
                            usbEndpoints[deviceIndex][interfaceIndex] = new UsbEndpoint[usbInterfaces[deviceIndex][interfaceIndex].EndpointCount];
                            usbInterface = usbInterfaces[deviceIndex][interfaceIndex];
                            for (endpointIndex = 0; endpointIndex < usbInterfaces[deviceIndex][interfaceIndex].EndpointCount; endpointIndex++)
                            {
                                usbEndpoints[deviceIndex][interfaceIndex][endpointIndex] = usbInterfaces[deviceIndex][interfaceIndex].GetEndpoint(endpointIndex);
                                if (usbEndpoints[deviceIndex][interfaceIndex][endpointIndex].Direction == Android.Hardware.Usb.UsbAddressing.Out)
                                {
                                    outputEndpoint = usbEndpoints[deviceIndex][interfaceIndex][endpointIndex];
                                }
                                else
                                {
                                    inputEndpoint = usbEndpoints[deviceIndex][interfaceIndex][endpointIndex];
                                }
                            }
                        }
                    }
                }
            }

            if (usbDevice != null && usbInterface != null && outputEndpoint != null && inputEndpoint != null)
            {
                PendingIntent mPermissionIntent = PendingIntent.GetBroadcast(this, 0, new Intent(ACTION_USB_PERMISSION), 0);
                IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
                RegisterReceiver(usbReceiver, filter);
                usbManager.RequestPermission(usbDevice, mPermissionIntent);
                bool hasPermision = usbManager.HasPermission(usbDevice);

                if (hasPermision)
                {
                    UsbDeviceConnection deviceConnection = usbManager.OpenDevice(usbDevices[0]);
                    if (deviceConnection.ClaimInterface(usbInterface, true))
                    {
                        byte[] buffer = new byte[] { 0x04, 0x90, 0x65, 0x64 };
                        count = deviceConnection.BulkTransfer(outputEndpoint, buffer, buffer.Length, 5000);
                        buffer = new byte[] { 0x04, 0x80, 0x65, 0x60 };
                        count = deviceConnection.BulkTransfer(outputEndpoint, buffer, buffer.Length, 5000);
                        deviceConnection.ReleaseInterface(usbInterface);
                        deviceConnection.Close();
                        deviceConnection.Dispose();
                    }
                }
            }

            MainPage_Portable.uIHandler.commonState.midi.PreInit((UsbManager)GetSystemService(Context.UsbService), usbReceiver);
            MainPage_Portable.uIHandler.commonState.midi.Init(MainPage_Portable, "INTEGRA-7", OutputSelector, InputSelector, this, 0, 0);
            MainPage_Portable.platform_specific = new object[] { usbManager, usbInterface, usbDevice, outputEndpoint, inputEndpoint };
            MainPage_Portable.uIHandler.ShowLibrarianPage();
        }

        public void NoteOn(byte currentChannel, byte noteNumber, byte velocity)
        {
            Int32 count = 0;
            usbManager = (UsbManager)MainPage_Portable.platform_specific[0];
            usbInterface = (UsbInterface)MainPage_Portable.platform_specific[1];
            usbDevice = (UsbDevice)MainPage_Portable.platform_specific[2];
            outputEndpoint = (UsbEndpoint)MainPage_Portable.platform_specific[3];
            inputEndpoint = (UsbEndpoint)MainPage_Portable.platform_specific[4];
            UsbDeviceConnection deviceConnection = usbManager.OpenDevice(usbDevice);
            if (deviceConnection.ClaimInterface(usbInterface, true))
            {
                byte[] buffer = new byte[] { 0x04, 0x90, 0x65, 0x64 };
                count = deviceConnection.BulkTransfer(outputEndpoint, buffer, buffer.Length, 5000);
                deviceConnection.ReleaseInterface(usbInterface);
                deviceConnection.Close();
                deviceConnection.Dispose();
            }
        }

        public void NoteOff(byte currentChannel, byte noteNumber)
        {
            Int32 count = 0;
            usbManager = (UsbManager)MainPage_Portable.platform_specific[0];
            usbInterface = (UsbInterface)MainPage_Portable.platform_specific[1];
            usbDevice = (UsbDevice)MainPage_Portable.platform_specific[2];
            outputEndpoint = (UsbEndpoint)MainPage_Portable.platform_specific[3];
            inputEndpoint = (UsbEndpoint)MainPage_Portable.platform_specific[4];
            UsbDeviceConnection deviceConnection = usbManager.OpenDevice(usbDevice);
            if (deviceConnection.ClaimInterface(usbInterface, true))
            {
                byte[] buffer = new byte[] { 0x04, 0x80, 0x65, 0x00 };
                count = deviceConnection.BulkTransfer(outputEndpoint, buffer, buffer.Length, 5000);
                deviceConnection.ReleaseInterface(usbInterface);
                deviceConnection.Close();
                deviceConnection.Dispose();
            }
        }
    }
}

