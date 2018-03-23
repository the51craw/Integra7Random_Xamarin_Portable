using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public partial class UIHandler
    {
        public void DrawToneEditorPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Edit tone  
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
            // |                      |                      |                     |Favorites|Add  |Remove|
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |Reset hanging  |Play  |  
            // |                      |                      |                     |----------------------|
            // |                      |                      |                     |keyrange |+12k | -12k |
            // |------------------------------------------------------------------------------------------|
            // | Keyboard                                                                                 |
            // |                                                                                          | Main row 1
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Create all controls ------------------------------------------------------------------------

            Button ToneEdit_NotYetImplemented = new Button();
            ToneEdit_NotYetImplemented.HorizontalOptions = LayoutOptions.FillAndExpand;
            ToneEdit_NotYetImplemented.VerticalOptions = LayoutOptions.FillAndExpand;
            ToneEdit_NotYetImplemented.Text = "Not yet implemented";
            ToneEdit_NotYetImplemented.Clicked += ToneEdit_NotYetImplemented_Clicked;

            // Add handlers -------------------------------------------------------------------------------

            void ToneEdit_NotYetImplemented_Clicked(object sender, EventArgs e)
            {
                mainStackLayout.Children.RemoveAt(0);
                ShowLibrarianPage();
            }

            // Assemble grids with controls ---------------------------------------------------------------

            // Assemble EditorStackLayout -----------------------------------------------------------------

            EditorStackLayout = new StackLayout();
            EditorStackLayout.Children.Add((new GridRow(0, new View[] { ToneEdit_NotYetImplemented })).Row);

            // Assemble LibrarianStackLayout --------------------------------------------------------------
        }

        public void ShowToneEditorPage()
        {
            if (EditorStackLayout == null)
            {
                DrawToneEditorPage();
            }
            mainStackLayout.Children.Add(EditorStackLayout);
        }
    }
}
