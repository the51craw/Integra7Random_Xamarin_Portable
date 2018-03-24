using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using INTEGRA_7_Xamarin.Droid;
using Android.Content;
using Android.Hardware.Usb;
using System.Collections.Generic;
using System;
using Android.Views;
using Android.Text.Method;
using static Android.Views.View;

//[assembly: Xamarin.Forms.Dependency(typeof(GenericHandlerInterface))]
//[assembly: Dependency(typeof(Android_MIDI))]

namespace INTEGRA_7_Xamarin.Droid
{
    [Activity(Label = "INTEGRA_7_Xamarin", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //[Android.Runtime.Register("setOnTouchListener", "(Landroid/view/View$OnTouchListener;)V", "GetSetOnTouchListener_Landroid_view_View_OnTouchListener_Handler")]
    [IntentFilter(new[] { UsbManager.ActionUsbDeviceAttached })]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity//, IOnTouchListener
    {

        private static String ACTION_USB_PERMISSION = "eu.mrmartin.MIDI.USB_PERMISSION";
        private static String USB_DEVICE_ATTACHED = "android.hardware.usb.action.USB_DEVICE_ATTACHED";
        private static String USB_DEVICE_DETACHED = "android.hardware.usb.action.USB_DEVICE_DETACHED";
        public PendingIntent mPermissionIntent = null;
        public UsbManager usbManager = null;
        public static UsbInterface usbInterface = null;
        public UsbDevice usbDevice = null;
        public UsbEndpoint outputEndpoint = null;
        public UsbEndpoint inputEndpoint = null;
        public USB usb = null;
        public PendingIntent pendingIntent;

        // For accessing INTEGRA_7_Xamarin.MainPage from UWP:
        private INTEGRA_7_Xamarin.MainPage MainPage_Portable;

        public MainActivity mainActivity;

        // Invisible comboboxes used by MIDI class (will always have INTEGRA-7 selected):
        private Picker OutputSelector;
        private Picker InputSelector;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            UIHandler.appType = UIHandler._appType.ANDROID;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            Init();
        }

        private void Init()
        {
            Xamarin.Forms.DependencyService.Register<IMidi>();

            // Get INTEGRA_7_Xamarin.MainPage:
            MainPage_Portable = INTEGRA_7_Xamarin.MainPage.GetMainPage();
            UIHandler.appType = UIHandler._appType.ANDROID;

            // Make Portable project draw UI and get the Pickers for Midi output device:
            MainPage_Portable.uIHandler.DrawLibrarianPage();
            OutputSelector = MainPage_Portable.uIHandler.Librarian_midiOutputDevice;
            InputSelector = MainPage_Portable.uIHandler.Librarian_midiInputDevice;

            // Let the portable project know this MainActivity:
            mainActivity = this;
            MainPage_Portable.SetDeviceSpecificMainPage(this);

            // Get and initiate USB:
            UsbManager usbManager = (UsbManager)GetSystemService(Context.UsbService);
            usb = new USB(usbManager, this);

            // Hook up USB :
            pendingIntent = PendingIntent.GetBroadcast(this, 0, new Intent(ACTION_USB_PERMISSION), 0);
            IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
            filter.AddAction(USB_DEVICE_ATTACHED);
            filter.AddAction(USB_DEVICE_DETACHED);
            RegisterReceiver(usb, filter);

            // Ask user for permission to use USB if creation and initiation was successful:
            if (usb.Device != null && usb.Interface != null && usb.OutputEndpoint != null && usb.InputEndpoint != null)
            {
                usb.Manager.RequestPermission(usb.Device, pendingIntent);
                usb.HasPermission = usb.Manager.HasPermission(usb.Device);
            }

            // Initiate MIDI:
            MainPage_Portable.uIHandler.commonState.midi.Init(MainPage_Portable, "INTEGRA-7", OutputSelector, InputSelector, this, 0, 0);

            // Let the portable project access our USB:
            MainPage_Portable.platform_specific = new object[] { usb };

            //Android.Views.View view = this.SetContentView()
            //mainActivity.Touch  
            //OnTouchListener += 
            //Android.Views.View view = FindViewById(Resource.Id.sliding_tabs);
            //view.SetOnTouchListener(this);    

            //SetOnListener(this);
            //mainActivity. SetOnTouchListener()

            // Show the librarian at startup:
            MainPage_Portable.uIHandler.ShowLibrarianPage();

            //TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            //tapGestureRecognizer.Tapped += (sender, e) => OnTouch((Android.Views.View)sender, (TappedEventArgs)e);
            //MainPage_Portable.uIHandler.Librarian_Keyboard.GestureRecognizers.Add(tapGestureRecognizer);

            //PanGestureRecognizer panGestureRecognizer = new PanGestureRecognizer();
            ////panGestureRecognizer.PanUpdated += panGestureRecognizer_PanUpdated;
            //MainPage_Portable.uIHandler.Librarian_Keyboard.GestureRecognizers.Add(panGestureRecognizer);


            //Image image = (Xamarin.Forms.Image)MainPage_Portable.uIHandler.Librarian_Keyboard. On<Xamarin.Forms.Image>();
        }

        //public bool OnTouch(Android.Views.View sender, TappedEventArgs e)
        //{
        //    //return ((IOnTouchListener)mainActivity).OnTouch(sender, e);
        //    return false;
        //}

        //public override bool OnTouchEvent(MotionEvent e)
        //{
        //    return base.OnTouchEvent(e);
        //}

        //private void SetOnTouchListener()
        //{

        //}

        //public bool OnTouchListener(Xamarin.Forms.View v, MotionEvent e)
        //{
        //    MainPage_Portable.uIHandler.x = e.GetX(0);
        //    MainPage_Portable.uIHandler.y = e.GetY(0);
        //    //return base.OnTouchEvent(e);
        //    return false;
        //}

        //public bool OnTouch(Android.Views.View v, MotionEvent e)
        //{
        //    MainPage_Portable.uIHandler.x = e.GetX(0);
        //    MainPage_Portable.uIHandler.y = e.GetY(0);
        //    //return base.OnTouchEvent(e);
        //    return false;
        //}

        //public override bool OnTouchEvent(MotionEvent e)
        //{
        //    MainPage_Portable.uIHandler.x = e.GetX(0);
        //    MainPage_Portable.uIHandler.y = e.GetY(0);
        //    return base.OnTouchEvent(e);
        //}
    }
}
