using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IncidentesAMT.Helpers;
using IncidentesAMT.View;

namespace IncidentesAMT
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            
            new NetworkState().iHaveInternet();

            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }



    }
}
