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
        private static MainPage MainPage_Portable;
        public object MainPage_Device { get; set; }
        public object[] platform_specific { get; set; }

        public interface IEventHandler
        {
            void GlobalHandler(object sender, EventArgs e);
        }
        
        public MainPage()
        {
            InitializeComponent();
            MainPage_Portable = this;
            Init();
        }

        public void Init()
        {
            StackLayout mainStackLayout = this.FindByName<StackLayout>("MainStackLayout");
            uIHandler = new UIHandler(mainStackLayout, this);
        }

        public static MainPage GetMainPage()
        {
            return MainPage_Portable;
        }

        public void SetDeviceSpecificMainPage(object mainPage)
        {
            MainPage_Device = mainPage;
        }
    }
}
