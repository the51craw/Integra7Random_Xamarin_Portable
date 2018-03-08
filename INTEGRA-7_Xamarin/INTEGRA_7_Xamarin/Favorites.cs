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
        public enum FavoriteAction
        {
            SHOW,
            ADD,
            REMOVE
        }

        public FavoriteAction favoriteAction = FavoriteAction.SHOW;

        public void DrawFavoritesPage()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Favorites
            // ____________________________________________________________________________________________
            // |                                                                                          |
            // |__________________________________________________________________________________________|

            // Create all controls ------------------------------------------------------------------------

            Button Favorites_NotYetImplemented = new Button();
            Favorites_NotYetImplemented.HorizontalOptions = LayoutOptions.FillAndExpand;
            Favorites_NotYetImplemented.VerticalOptions = LayoutOptions.FillAndExpand;
            Favorites_NotYetImplemented.Text = "Not yet implemented";
            Favorites_NotYetImplemented.Clicked += Favorites_NotYetImplemented_Clicked;

            // Add handlers -------------------------------------------------------------------------------

            // Assemble grids with controls ---------------------------------------------------------------

            // Assemble FavoritesStackLayout -----------------------------------------------------------------

            FavoritesStackLayout = new StackLayout();
            FavoritesStackLayout.Children.Add((new GridRow(0, new View[] { Favorites_NotYetImplemented })).Row);
        }

        private void Favorites_NotYetImplemented_Clicked(object sender, EventArgs e)
        {
            mainStackLayout.Children.RemoveAt(0);
            //ShowLibrarianPage();
        }

        public void ShowFavoritesPage(FavoriteAction favoriteAction)
        {
            mainStackLayout.Children.Add(FavoritesStackLayout);
        }
    }
}
