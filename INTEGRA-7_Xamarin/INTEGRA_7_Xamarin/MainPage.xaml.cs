using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public partial class MainPage : ContentPage
    {
        public UIHandler uIHandler;
        private static MainPage mainPage;

        public interface IEventHandler
        {
            void GlobalHandler(object sender, EventArgs e);
        }
        
        public MainPage()
        {
            InitializeComponent();
            mainPage = this;
            Init();
        }

        public void Init()
        {
            StackLayout mainStackLayout = this.FindByName<StackLayout>("MainStackLayout");
            uIHandler = new UIHandler(mainStackLayout, this);
        }

        public static MainPage GetMainPage()
        {
            return mainPage;
        }
    }
}
