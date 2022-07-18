using Acr.UserDialogs;
using IncidentesAMT.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static IncidentesAMT.Model.SendMailModel;

namespace IncidentesAMT.ViewModel
{
    public class RecuperarPasswordViewModel : BaseViewModel
    {
        ResetPass obj;
        #region COMANDOS
        public Command ResetCommand { get; set; }

        #endregion

        #region PROPIEDADES

        private INavigation Navigation { get; set; }

        private string _identificacion;

        public string Identificacion
        {
            get { return _identificacion; }
            set { _identificacion = value; OnPropertyChanged(); }
        }
        #endregion

        public RecuperarPasswordViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ResetCommand = new Command(() => ResetPassword());
        }

        public async void ResetPassword()
        {
            try
            {
                if (string.IsNullOrEmpty(Identificacion))
                {
                    await DisplayAlert("Error", "Debe ingresar un número de cédula", "Cerrar");
                    return;
                }
                if (!Verify_Ci.VerificaIdentificacion(Identificacion))
                {
                    await DisplayAlert("Error", "El número de cédula ingresado es incorrecto", "Cerrar");
                    return;
                }
                UserDialogs.Instance.ShowLoading("Recuperando contraseña...");

                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://servicios.amt.gob.ec:5001/personas/recuperarPassword/" + Identificacion);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accpet", "application/json");
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    UserDialogs.Instance.HideLoading();
                    string content = await response.Content.ReadAsStringAsync();
                    obj = JsonConvert.DeserializeObject<ResetPass>(content);
                    if(await SendWhatsApp())
                    {
                        await DisplayAlert("Mensaje", "Se ha reestablecido su contraseña, revise su correo electrónico o WhatsApp", "Ok");
                        Application.Current.MainPage = new NavigationPage(new Login());
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Ocurrió un error al intentar recuperar su contraseña", "Cerrar");
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", $"Ocurrión un error,{ex.Message}", "Cerrar");
            }
        }

        private async Task<bool> SendWhatsApp()
        {
            try
            {
                SendWhatsApp sendWhatsApp = new SendWhatsApp
                {
                    celular = Convert.ToString(obj.telefono),
                    message = $"Estimad@ {obj.nombresCompletos} su nueva contraseña es {obj.password}, por favor actualice su contraseña lo antes posible"
                };
                Uri RequestUri = new Uri("http://servicios.amt.gob.ec:5001/personas/enviarSMS");
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(sendWhatsApp);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Ocurrión un error al enviar su contraseña por WhatsApp", "Cerrar");
                return false;
            }
           
        }
    }
}
