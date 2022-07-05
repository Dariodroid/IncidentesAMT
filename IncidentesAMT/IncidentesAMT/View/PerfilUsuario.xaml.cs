using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using IncidentesAMT.View;
using IncidentesAMT.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public PerfilUsuario(string idUser, ObservableCollection<IncidenteByPersonaModel> IncidenteByPersonaModel)
        {
            InitializeComponent();
            _idUser = idUser;
            BindingContext = new PerfilUsuarioViewModel(Navigation,_idUser, IncidenteByPersonaModel);
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