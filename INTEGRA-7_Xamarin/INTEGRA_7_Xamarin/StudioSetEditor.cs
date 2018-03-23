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
        public void DrawStudioSetEditorPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Edit studio set
            // ____________________________________________________________________________________________
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Create all controls ------------------------------------------------------------------------

            Button StudioSetEditor_NotYetImplemented = new Button();
            StudioSetEditor_NotYetImplemented.HorizontalOptions = LayoutOptions.FillAndExpand;
            StudioSetEditor_NotYetImplemented.VerticalOptions = LayoutOptions.FillAndExpand;
            StudioSetEditor_NotYetImplemented.Text = "Not yet implemented";
            StudioSetEditor_NotYetImplemented.Clicked += StudioSetEditor_NotYetImplemented_Clicked; ;

            // Add handlers -------------------------------------------------------------------------------

            // Assemble grids with controls ---------------------------------------------------------------

            // Assemble StudioSetEditorStackLayout -----------------------------------------------------------------

            StudioSetEditorStackLayout = new StackLayout();
            StudioSetEditorStackLayout.Children.Add((new GridRow(0, new View[] { StudioSetEditor_NotYetImplemented })).Row);
        }

        private void StudioSetEditor_NotYetImplemented_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            ShowLibrarianPage();
        }

        public void ShowStudioSetEditorPage()
        {
            if (StudioSetEditorStackLayout == null)
            {
                DrawStudioSetEditorPage();
            }
            mainStackLayout.Children.Add(StudioSetEditorStackLayout);
        }
    }
 }
