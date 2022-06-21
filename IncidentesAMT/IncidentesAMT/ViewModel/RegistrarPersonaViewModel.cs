using IncidentesAMT.Helpers;
using IncidentesAMT.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace IncidentesAMT.ViewModel
{
    public class RegistrarPersonaViewModel : BaseViewModel
    {
        bool verifcado = false;

        #region PROPIEDADES
        INavigation Navigation { get; set; }
        public Command NextPageCommand { get; set; }
        public Command ScannCommand { get; set; }

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
                    celular = celular
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

        private async Task ScannCI()
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
                    Identificacion = result.Text;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PopModalAsync();
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

    }
}
