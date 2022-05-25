using IncidentesAMT.View;
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
        string _idUser;
        public PerfilUsuario(string idUser)
        {
            InitializeComponent();
            _idUser = idUser;
        }

        private async void btnDatosPersonales_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DatosPersonales(_idUser));
        }

        private async void btnCerrarSesion_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("UserId");
            Preferences.Clear();
            //var login = new Login();
            await Navigation.PushAsync(new Login(), true);
        }

        private async void btnIncidentes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IncidenteByUsuario(_idUser));
        }
    }
}