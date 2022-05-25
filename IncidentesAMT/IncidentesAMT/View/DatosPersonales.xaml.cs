using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using IncidentesAMT.VistaModelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncidentesAMT.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatosPersonales : ContentPage
    {
        string _idUser;
        private InfoUserByIdModel infoUserModel;
        FotoViewModel fotoViewModel;
        string foto;
        public DatosPersonales(string idUser)
        {
            InitializeComponent();
            _idUser = idUser;
            GetPersonaById();
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

        private void btnFoto_Clicked(object sender, EventArgs e)
        {
            takefoto();
        }
        private async void takefoto()
        {
            fotoViewModel = new FotoViewModel();
            await fotoViewModel.TomarFoto();
            foto = fotoViewModel.PathFoto;
            fotocedula.Source = foto;
        }

        private void btnActualizar_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}