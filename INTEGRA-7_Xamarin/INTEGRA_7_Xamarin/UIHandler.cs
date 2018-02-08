using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public class UIHandler
    {
        public enum _appType
        {
            UWP,
            IOS,
            MacOS,
            ANDROID,
        }

        enum _page
        {
            MAIN,
            SEARCH_RESULTS,
            FAVORITES,
            EDIT,
        }

        IMidi midi;

        INTEGRA_7_Xamarin.MainPage mainPage;
        StackLayout mainStackLayout { get; set; }
        public static _appType appType;
        public static ColorSettings colorSettings { get; set; }
        _page page;
        public Picker midiOutputDevice { get; set; }
        public Picker midiInputDevice { get; set; }
        //public LabeledPicker midiOutputDevicePicker { get; set; }
        //public LabeledPicker midiInputDevicePicker { get; set; }
        public Picker midiOutputChannel { get; set; }
        public Picker midiInputChannel { get; set; }

        LabeledTextInput tbSearch;
        LabeledText ltToneName;
        LabeledText ltType;
        LabeledText ltToneNumber;
        LabeledText ltBankNumber;
        LabeledText ltBankMSB;
        LabeledText ltBankLSB;
        LabeledText ltProgramNumber;

        Image Keyboard;

        public UIHandler(StackLayout mainStackLayout, INTEGRA_7_Xamarin.MainPage mainPage)
        {
            this.mainStackLayout = mainStackLayout;
            this.mainPage = mainPage;
            page = _page.MAIN;
            colorSettings = new ColorSettings(_colorSettings.LIGHT);
        }

        public void Clear()
        {
            while (mainStackLayout.Children.Count() > 0)
            {
                mainStackLayout.Children.RemoveAt(0);
            }
        }

        public void DrawPage()
        {
            Clear();
            switch (page)
            {
                case _page.MAIN:
                    DrawMain();
                    break;
            }
        }

        public void DrawMain()
        {
            //                                                           ____________________________________ gridTones
            //                                                          /                      ______________ gridToneData
            //                                                         /                      /
            // ____________________________________________________________________________________________
            // | lvGroups             | lvCategories         | filterPresetAndUser |midiOutputDevice/part |
            // |                      |                      |---------------------|----------------------|
            // |                      |                      | lvToneNames         |tbSearch              |
            // |                      |                      |                     |----------------------|
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
            // |                      |                      |                     |Add fav. |Remove fav. |
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |Play     |Favorites   |  
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |keyrange |+12k | -12k |
            // |------------------------------------------------------------------------------------------|
            // | Keyboard                                                                                 |
            // |                                                                                          | Main row 1
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Make a listview lvGroups for column 0:
            ListView lvGroups = new ListView();

            // Make a listview lvCategories for column 1:
            ListView lvCategories = new ListView();
            List<String> lv = new List<String>();
            for (Int32 i = 0; i < 50; i++)
            {
                lv.Add("Category number " + i.ToString());
            }
            lvCategories.ItemsSource = lv;

            // Make a Grid for column 2:
            Grid gridTones = new Grid();

            // Make a filter button for column 2:
            Button filterPresetAndUser = new Button();
            filterPresetAndUser.Text = "Preset and User";
            filterPresetAndUser.BackgroundColor = colorSettings.Background;

            // Make a listview lvToneNames for column 2:
            ListView lvToneNames = new ListView();
            lvToneNames.BackgroundColor = colorSettings.Background;
            lvToneNames.ItemsSource = lv;

            // Make a Grid for column 3:
            Grid gridToneData = new Grid();

            // Make pickers for MIDI:
            midiOutputDevice = new Picker();
            midiInputDevice = new Picker();
            midiOutputChannel = new Picker();
            for (Int32 i = 0; i < 16; i++)
            {
                String temp = "Part " + (i + 1).ToString();
                midiOutputChannel.Items.Add(temp);
            }
            midiOutputChannel.SelectedIndex = 0;
            midiInputChannel = new Picker();
            for (Int32 i = 0; i < 16; i++)
            {
                String temp = "Part " + (i + 1).ToString();
                midiInputChannel.Items.Add(temp);
            }
            midiInputChannel.SelectedIndex = 0;
            midiInputDevice.IsVisible = false;
            midiInputChannel.IsVisible = false;

            // Make labeled editor fields:
            tbSearch = new LabeledTextInput("Search:", new byte[] { 1, 2 });
            ltToneName = new LabeledText("Tone Name:", "Full Grand 1", new byte[] { 1, 2 });
            ltType = new LabeledText("Type:", "(Preset)", new byte[] { 1, 2 });
            ltToneNumber = new LabeledText("Tone #:", "1", new byte[] { 1, 2 });
            ltBankNumber = new LabeledText("Bank #:", "11456", new byte[] { 1, 2 });
            ltBankMSB = new LabeledText("Bank MSB:", "89", new byte[] { 1, 2 });
            ltBankLSB = new LabeledText("Bank LSB:", "64", new byte[] { 1, 2 });
            ltProgramNumber = new LabeledText("Program #:", "1", new byte[] { 1, 2 });

            // Add the keyboard image:
            Keyboard = new Image { Aspect = Aspect.Fill };
            Keyboard.Source = ImageSource.FromFile("Keyboard.jpg");
            Keyboard.HeightRequest = 330;
            Keyboard.VerticalOptions = LayoutOptions.StartAndExpand;
            Keyboard.HorizontalOptions = LayoutOptions.CenterAndExpand;
            mainStackLayout.Children.Add(Keyboard);

            // Columns 0 and 1 are only one listview each and thus has nothing to assemble:
            // lvGroups and lvCategories 

            // Assemble column 2:
            gridTones.Children.Add((new GridRow(0, new View[] { filterPresetAndUser })).Row);
            gridTones.Children.Add((new GridRow(1, new View[] { lvToneNames })).Row);
            gridTones.RowDefinitions = new RowDefinitionCollection();
            gridTones.RowDefinitions.Add(new RowDefinition());
            gridTones.RowDefinitions.Add(new RowDefinition());
            gridTones.RowDefinitions[0].Height = new GridLength(30, GridUnitType.Absolute);
            gridTones.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);

            // Assemble column 3:
            gridToneData.Children.Add((new GridRow(0, new View[] { midiOutputDevice, midiInputDevice, midiOutputChannel, midiInputChannel }, new byte[] { 255, 1, 255, 1 })).Row);
            gridToneData.Children.Add((new GridRow(1, new View[] { tbSearch })).Row);
            gridToneData.Children.Add((new GridRow(2, new View[] { ltToneName })).Row);
            gridToneData.Children.Add((new GridRow(3, new View[] { ltType })).Row);
            gridToneData.Children.Add((new GridRow(4, new View[] { ltToneNumber })).Row);
            gridToneData.Children.Add((new GridRow(5, new View[] { ltBankNumber })).Row);
            gridToneData.Children.Add((new GridRow(6, new View[] { ltBankMSB })).Row);
            gridToneData.Children.Add((new GridRow(7, new View[] { ltBankLSB })).Row);
            gridToneData.Children.Add((new GridRow(8, new View[] { ltProgramNumber })).Row);

            // Assemble mainStackLayout:
            mainStackLayout.Children.Add((new GridRow(0, new View[] { lvGroups, lvCategories, gridTones, gridToneData }, new byte[] { 1, 1, 1, 1 })).Row);
            mainStackLayout.Children.Add((new GridRow(1, new View[] { Keyboard })).Row);
            ((Grid)mainStackLayout.Children[0]).RowDefinitions = new RowDefinitionCollection();
            ((Grid)mainStackLayout.Children[0]).RowDefinitions.Add(new RowDefinition());
            ((Grid)mainStackLayout.Children[0]).RowDefinitions.Add(new RowDefinition());
            ((Grid)mainStackLayout.Children[0]).RowDefinitions[0].Height = new GridLength(8, GridUnitType.Star);
            ((Grid)mainStackLayout.Children[0]).RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ltToneName.Text.Text = (String)((Picker)(sender)).SelectedItem;
        }
    }
}
