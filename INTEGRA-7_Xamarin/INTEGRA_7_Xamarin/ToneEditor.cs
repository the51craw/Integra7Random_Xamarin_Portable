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
        Grid Edit_grEditor = null;
        Grid Edit_grHelp = null;
        Grid Edit_grCommon = null;
        Grid Edit_grControls = null;
        Picker cbEditTone_PartSelector = null;
        Picker cbEditTone_SynthesizerType = null;
        Button tbEditTone_Instrument = null;
        Picker cbEditTone_ParameterPages = null;
        Picker cbEditTone_PartialSelector = null;
        Button tbEditTone_KeyName = null;
        Picker cbEditTone_KeySelector = null;
        Picker cbEditTone_InstrumentCategorySelector = null;

        public void DrawToneEditorPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Edit tone  
            // -------------------------------------------------------------------------------------------
            // | Edit_grEdit                                        |  Edit_grHelp                       |
            // |----------------------------------------------------|                                    |
            // ||Edit_grCommon                                     ||                                    |
            // ||--------------------------------------------------||                                    |
            // ||cbPart          | cbTone         | lblPreset      ||                                    |
            // ||--------------------------------------------------||                                    |
            // ||cbCommon        | cbPartial      | cbToneCategory ||                                    |
            // ||--------------------------------------------------||                                    |
            // ||| Edit_grControls                                |||                                    |
            // ||--------------------------------------------------||                                    |
            // |----------------------------------------------------|                                    |
            // -------------------------------------------------------------------------------------------

            // Create all controls ------------------------------------------------------------------------

            Button ToneEdit_NotYetImplemented = new Button();
            ToneEdit_NotYetImplemented.HorizontalOptions = LayoutOptions.FillAndExpand;
            ToneEdit_NotYetImplemented.VerticalOptions = LayoutOptions.FillAndExpand;
            ToneEdit_NotYetImplemented.Text = "Not yet implemented";
            ToneEdit_NotYetImplemented.Clicked += ToneEdit_NotYetImplemented_Clicked;

            Edit_grEditor = new Grid();
            Edit_grEditor.HorizontalOptions = LayoutOptions.FillAndExpand;
            Edit_grEditor.VerticalOptions = LayoutOptions.FillAndExpand;

            Edit_grHelp = new Grid();
            Edit_grHelp.HorizontalOptions = LayoutOptions.FillAndExpand;
            Edit_grHelp.VerticalOptions = LayoutOptions.FillAndExpand;

            Edit_grCommon = new Grid();
            Edit_grCommon.HorizontalOptions = LayoutOptions.FillAndExpand;
            Edit_grCommon.VerticalOptions = LayoutOptions.FillAndExpand;

            Edit_grControls = new Grid();
            Edit_grControls.HorizontalOptions = LayoutOptions.FillAndExpand;
            Edit_grControls.VerticalOptions = LayoutOptions.FillAndExpand;

            cbEditTone_PartSelector = new Picker();
            cbEditTone_PartSelector.HorizontalOptions = LayoutOptions.FillAndExpand;
            cbEditTone_PartSelector.VerticalOptions = LayoutOptions.FillAndExpand;

            cbEditTone_SynthesizerType = new Picker();
            cbEditTone_SynthesizerType.HorizontalOptions = LayoutOptions.FillAndExpand;
            cbEditTone_SynthesizerType.VerticalOptions = LayoutOptions.FillAndExpand;

            tbEditTone_Instrument = new Button();
            tbEditTone_Instrument.HorizontalOptions = LayoutOptions.FillAndExpand;
            tbEditTone_Instrument.VerticalOptions = LayoutOptions.FillAndExpand;

            cbEditTone_ParameterPages = new Picker();
            cbEditTone_ParameterPages.HorizontalOptions = LayoutOptions.FillAndExpand;
            cbEditTone_ParameterPages.VerticalOptions = LayoutOptions.FillAndExpand;

            cbEditTone_PartialSelector = new Picker();
            cbEditTone_PartialSelector.HorizontalOptions = LayoutOptions.FillAndExpand;
            cbEditTone_PartialSelector.VerticalOptions = LayoutOptions.FillAndExpand;

            tbEditTone_KeyName = new Button();
            tbEditTone_KeyName.HorizontalOptions = LayoutOptions.FillAndExpand;
            tbEditTone_KeyName.VerticalOptions = LayoutOptions.FillAndExpand;

            cbEditTone_KeySelector = new Picker();
            cbEditTone_KeySelector.HorizontalOptions = LayoutOptions.FillAndExpand;
            cbEditTone_KeySelector.VerticalOptions = LayoutOptions.FillAndExpand;

            cbEditTone_InstrumentCategorySelector = new Picker();
            cbEditTone_InstrumentCategorySelector.HorizontalOptions = LayoutOptions.FillAndExpand;
            cbEditTone_InstrumentCategorySelector.VerticalOptions = LayoutOptions.FillAndExpand;


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

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Tone editor handlers
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ShowToneEditorPage()
        {
            if (EditorStackLayout == null)
            {
                DrawToneEditorPage();
            }
            mainStackLayout.Children.Add(EditorStackLayout);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Tone editor functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Tone editor helpers
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
