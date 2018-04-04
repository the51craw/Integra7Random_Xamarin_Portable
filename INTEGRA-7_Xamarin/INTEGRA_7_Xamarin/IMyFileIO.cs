using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace INTEGRA_7_Xamarin
{
    public interface IMyFileIO
    {
        void SetMainPagePortable(INTEGRA_7_Xamarin.MainPage mainPage);
        void LoadFavorites();
    }
}
