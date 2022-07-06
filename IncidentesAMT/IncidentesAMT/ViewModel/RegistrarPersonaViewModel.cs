using Acr.UserDialogs;
using IncidentesAMT.Helpers;
using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace IncidentesAMT.ViewModel
{
    public class RegistrarPersonaViewModel : BaseViewModel
    {
        #region VARIABLES
        bool verifcado = false;
        DataCiModel.Root dataCiModel;
        #endregion

        #region COMANDOS
        public Command NextPageCommand { get; set; }
        public Command ScannCommand { get; set; }
        #endregion

        #region PROPIEDADES
        INavigation Navigation { get; set; }

        private string identificacion;
        private string nombres;
        private string apellidos;
        private string correo;
        private string celular;
        public string Identificacion
        {
            get { return identificacion; }
            set
            {
                identificacion = value;
                if(Identificacion.Length == 10)
                {
                    if (Verify_Ci.VerificaIdentificacion(Identificacion))
                        GetDataCI(Identificacion);
                    else
                        DisplayAlert("Error", "Cédula incorrecta", "Ok");
                }
                OnPropertyChanged();
            }
        }
        public string Nombres
        {
            get { return nombres; }
            set
            {
                nombres = value;
                OnPropertyChanged();
            }
        }
        public string Apellidos
        {
            get { return apellidos; }
            set
            {
                apellidos = value;
                OnPropertyChanged();
            }
        }
        public string Correo
        {
            get { return correo; }
            set
            {
                correo = value;
                OnPropertyChanged();
            }
        }
        public string Celular
        {
            get { return celular; }
            set
            {
                celular = value;
                OnPropertyChanged();
            }
        }
        #endregion      

        public RegistrarPersonaViewModel(INavigation navigation)
        {
            Navigation = navigation;
            NextPageCommand = new Command(() => NextPage());
            ScannCommand = new Command(async() => await ScannCI());
        }

        private async void NextPage()
        {
            if (!verifcado)
            {
                bool opcion = await DisplayAlert("Alerta !", "No se realizó el escaneo de cédula; sus incidentes reportados pueden no se atendidos, ¿ Realizar escaneo para verificar su cédula ?", "Si", "No");
                if (opcion) { await ScannCI(); }
            }

            if (!Verify_Email())
            {
                await DisplayAlert("Error", "Ingrese un correo electrónico válido", "Ok");
                return;
            }

            if (celular.Length < 10)
            {
                await DisplayAlert("Error", "Ingrese un número celular válido", "Ok");
                return;
            }
            
            if (await VerifyData() && await VerifyCI())
            {
                PersonaModel persona = new PersonaModel
                {
                    identificacion = identificacion,
                    nombres = nombres,
                    apellidos = apellidos,
                    correo = correo,
                    telefono = celular
                };
                await Navigation.PushAsync(new Registro(persona, verifcado));
            }
        }

        private bool Verify_Email()
        {
            return Verify_email.Verify(correo);
        }

        private async Task<bool> VerifyData()
        {            
            if (string.IsNullOrEmpty(Identificacion))
            {
                await DisplayAlert("Error", "Debe ingresar su cédula", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(Nombres))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(Apellidos))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(Correo))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(Celular))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                return false;
            }
            return true;
        }

        private async Task<bool> VerifyCI()
        {
            if (!Verify_Ci.VerificaIdentificacion(Identificacion))
            {
                await DisplayAlert("Ocurrió un error", "La cédula es incorrecta", "Cerrar");
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task ScannCI()
        {

            var options = new MobileBarcodeScanningOptions();

            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = false,
                TopText = "Coloca el código de barra de tu cédula frente al dispositivo",
                BottomText = "El escaneo es automático",
                Opacity = 0.90,
            }; overlay.BindingContext = overlay;

            var page = new ZXingScannerPage(options, overlay)
            {
                Title = "Escaneo Cédula",
                DefaultOverlayShowFlashButton = true,
            };

            await Navigation.PushModalAsync(page);

            page.OnScanResult += (result) =>
            {
                page.IsScanning = false;
                page.IsAnalyzing = false;
                verifcado = Verify_Ci.VerificaIdentificacion(result.Text);
                if (verifcado)
                {
                    Vibration.Vibrate();
                    var time = TimeSpan.FromMilliseconds(100);
                    Vibration.Vibrate(time);
                    Identificacion = result.Text;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PopModalAsync();
                        GetDataCI(Identificacion);
                        await DisplayAlert("Mensaje", "Cédula verificada correctamente", "Ok");
                    });
                }
                else
                {
                    overlay.TopText = "El documento es incorrecto".ToUpper();
                    overlay.BottomText = "Verifique la iluminación y distancia del documento";
                    overlay.BindingContext = overlay;
                }
            };

        }

        public async void GetDataCI(string Ci)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando sus datos...");
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://servicios.amt.gob.ec:5001/obtenerDatos/" + Ci);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accpet", "application/json");
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dataCiModel = JsonConvert.DeserializeObject<DataCiModel.Root>(content);
                    Nombres = dataCiModel.retorno.nombres;
                    Apellidos = $"{dataCiModel.retorno.apellido1} {dataCiModel.retorno.apellido2}";
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    await DisplayAlert("No se encontraron datos", "Ingrese sus datos manualmente", "Ok");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message.ToString(), "ok");
            }
        }
    }
}
