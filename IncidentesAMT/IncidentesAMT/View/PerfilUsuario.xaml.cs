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

        private void Button_Clicked(object sender, EventArgs e)
        {
            sendMail("Acuerdo de responsabilidad", "cuerpo de correo ");
        }

        private async void sendMail(string subject, string body)
        {
            try
            {
                SendMailModel sendMail = new SendMailModel()
                {
                    to = lblCorreo.Text,
                    subject = subject,
                    html = body,
                };

                Uri RequestUri = new Uri("http://servicios.amt.gob.ec:4001/api/mail/send");
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(sendMail);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await DisplayAlert("Mensaje", "Correo enviado", "Ok");
                    //await Navigation.PushAsync(new Login(), true);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cerrar");
            }
        }
    }
}