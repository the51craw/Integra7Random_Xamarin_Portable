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
        public MainPage mainPage { get; set; }

        public void GenericHandler(object sender, object e)
        {
            if (mainPage.midi.midiOutPort == null)
            {
                mainPage.midi.Init("INTEGRA-7");
            }
        }
    }

    public sealed partial class MainPage
    {
        // For accessing INTEGRA_7_Xamarin.MainPage from UWP:
        public INTEGRA_7_Xamarin.MainPage MainPage_Portable { get; set; }
        public INTEGRA_7_Xamarin.UWP.MainPage MainPage_UWP { get; set; }

        // Invisible comboboxes used by MIDI class (will always have INTEGRA-7 selected):
        private Picker OutputSelector;
        private Picker InputSelector;
        public MIDI midi;
        public Keyboard keyboard;
        //private Double x, y;

        // For accessing the genericHandlerInterface:
        GenericHandlerInterface genericHandlerInterface;
        public Windows.UI.Core.CoreDispatcher Dispatcher_UWP { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new INTEGRA_7_Xamarin.App());
            Init();
        }
        
        private void Init()
        {
            // Get dispatcher:
            Dispatcher_UWP = Dispatcher;

            // Get INTEGRA_7_Xamarin.MainPage:
            MainPage_Portable = INTEGRA_7_Xamarin.MainPage.GetMainPage();
            UIHandler.appType = UIHandler._appType.UWP;

            // Get the generic handler (same way as done in INTEGRA_7_Xamarin.UIHandler):
            genericHandlerInterface = (INTEGRA_7_Xamarin.UWP.GenericHandlerInterface)DependencyService.Get<IGenericHandler>();

            // Let genericHandlerInterface know this MainPage:
            genericHandlerInterface.mainPage = this;

            // Let portable know this MainPage:
            MainPage_Portable.MainPage_Device = this;

            // Draw UI (function is in mainPage.uIHandler):
            MainPage_Portable.uIHandler.DrawPages();

            // We need invisible ComboBoxes to hold settings from the
            // corresponding Pickers in the Xamarin code.
            OutputSelector = MainPage_Portable.uIHandler.Librarian_midiOutputDevice;
            InputSelector = MainPage_Portable.uIHandler.Librarian_midiInputDevice;

            MainPage_Portable.SetDeviceSpecificMainPage(this);

            MainPage_Portable.uIHandler.commonState.midi.Init(MainPage_Portable, "INTEGRA-7", OutputSelector, InputSelector, (object)Dispatcher_UWP, 0, 0);

            // Always start by showing librarian:
            MainPage_Portable.uIHandler.ShowLibrarianPage();

            //keyboard = new Keyboard(MainPage_Portable.uIHandler);
            //AddMouseHandlers();
        }

        //private void AddMouseHandlers()
        //{
        //    var window = Windows.UI.Core.CoreWindow.GetForCurrentThread();
        //    window.PointerPressed += Window_PointerPressed;
        //}

        //public double GetMouseX()
        //{
        //    //var window = Windows.UI.Core.CoreWindow.GetForCurrentThread();
        //    //window.PointerPressed += Window_PointerPressed;
        //    //    PointerPressed
        //    //System.Windows.Forms.Cursor.Position;
        //    //MouseState ms = Mouse.GetState();

        //    //Pointer mouse = Windows.ge
        //    return x;
        //}

        //public double GetMouseY()
        //{
        //    //var window = Windows.UI.Core.CoreWindow.GetForCurrentThread();
        //    //window.PointerPressed += Window_PointerPressed;
        //    //    PointerPressed
        //    //System.Windows.Forms.Cursor.Position;
        //    //MouseState ms = Mouse.GetState();

        //    //Pointer mouse = Windows.ge
        //    return y;
        //}

        //private void Window_PointerPressed(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.PointerEventArgs args)
        //{
        //    MainPage_Portable.uIHandler.x = sender.PointerPosition.X;
        //    MainPage_Portable.uIHandler.y = sender.PointerPosition.Y;
        //}

        public Windows.UI.Core.CoreDispatcher GetDispatcher()
        {
            return Dispatcher_UWP;
        }
    }
}
