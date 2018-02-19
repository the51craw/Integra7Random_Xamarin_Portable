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
        private INTEGRA_7_Xamarin.MainPage mainPage;

        // Invisible comboboxes used by MIDI class (will always have INTEGRA-7 selected):
        private Picker OutputSelector;
        private Picker InputSelector;
        public MIDI midi;
        
        // For accessing the genericHandlerInterface:
        GenericHandlerInterface genericHandlerInterface;
        public Windows.UI.Core.CoreDispatcher dispatcher;

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
            OutputSelector = mainPage.uIHandler.Librarian_midiOutputDevice;
            InputSelector = mainPage.uIHandler.Librarian_midiInputDevice;
        }

        private void Btn0_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
