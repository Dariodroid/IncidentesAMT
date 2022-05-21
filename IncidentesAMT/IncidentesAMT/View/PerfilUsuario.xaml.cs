using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncidentesAMT.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilUsuario : ContentPage
    {
        public PerfilUsuario()
        {
            InitializeComponent();
        }

        private async void btnDatosPersonales_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DatosPersonales());
        }

        private async void btnCerrarSesion_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("UserId");
            Preferences.Clear();
            var login = new Login();
            await Navigation.PushAsync(login,true);
        }
    }
}