using IncidentesAMT.Model;
using IncidentesAMT.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private InfoUserByIdModel infoUserModel;
        public PerfilUsuario(string idUser)
        {
            InitializeComponent();
            _idUser = idUser;
            infoUserModel = new InfoUserByIdModel();
            GetPersonaById();            
        }

        private async void btnDatosPersonales_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DatosPersonales(_idUser));
        }

        private async void btnCerrarSesion_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("UserId");
            Preferences.Clear();
            await Navigation.PushAsync(new Login(), true);
        }

        private async void btnIncidentes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IncidenteByUsuario(_idUser));
        }

        private async void GetPersonaById()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://incidentes-amt.herokuapp.com/personas/" + _idUser);
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accpet", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                infoUserModel = JsonConvert.DeserializeObject<InfoUserByIdModel>(content);
                BindingContext = infoUserModel;
            }
        }
    }
}