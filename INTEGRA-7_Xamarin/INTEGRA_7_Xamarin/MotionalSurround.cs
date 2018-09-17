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
        public void DrawMotionalSurroundPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Surround
            // ____________________________________________________________________________________________
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Create all controls ------------------------------------------------------------------------

            Button MotionalSurround_NotYetImplemented = new Button();
            MotionalSurround_NotYetImplemented.HorizontalOptions = LayoutOptions.FillAndExpand;
            MotionalSurround_NotYetImplemented.VerticalOptions = LayoutOptions.FillAndExpand;
            MotionalSurround_NotYetImplemented.Text = "Not yet implemented";
            MotionalSurround_NotYetImplemented.Clicked += MotionalSurround_NotYetImplemented_Clicked;

            // Add handlers -------------------------------------------------------------------------------

            // Assemble grids with controls ---------------------------------------------------------------

            // Assemble MotionalSurroundStackLayout -----------------------------------------------------------------

            MotionalSurroundStackLayout = new StackLayout();
            MotionalSurroundStackLayout.Children.Add((new GridRow(0, new View[] { MotionalSurround_NotYetImplemented })).Row);
        }

        private void MotionalSurround_NotYetImplemented_Clicked(object sender, EventArgs e)
        {
            //mainStackLayout.Children.RemoveAt(0);
            MotionalSurroundStackLayout.IsVisible = false;
            ShowLibrarianPage();
        }

        public void ShowMotionalSurroundPage()
        {
            if (!MotionalSurroundIsCreated)
            {
                DrawMotionalSurroundPage();
                mainStackLayout.Children.Add(MotionalSurroundStackLayout);
                MotionalSurroundIsCreated = true;
            }
            //mainStackLayout.Children.Add(MotionalSurroundStackLayout);
            MotionalSurroundStackLayout.IsVisible = true;
        }
    }
}
