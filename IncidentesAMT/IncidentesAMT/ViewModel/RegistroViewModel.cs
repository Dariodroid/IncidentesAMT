using Acr.UserDialogs;
using IncidentesAMT.Helpers;
using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using IncidentesAMT.View;
using IncidentesAMT.VistaModelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace IncidentesAMT.ViewModel
{
    public class RegistroViewModel : BaseViewModel
    {
        bool _verifcado = false;

        #region PROPIEDDADES
        private string _identificacion;
        private string _nombres;
        private string _apellidos;
        private string _correo;
        private string _nacionalidad;
        private string _celular;
        FotoViewModel fotoViewModel;
        string foto;

        INavigation Navigation { get; set; }

        private ImageSource _fotocedula;

        public ImageSource FotoCedula
        {
            get { return _fotocedula; }
            set { _fotocedula = value; OnPropertyChanged(); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _confirmPass;

        public string ConfirmPass
        {
            get { return _confirmPass; }
            set { _confirmPass = value; OnPropertyChanged(); }
        }

        private bool _isacuerdo;

        public bool IsAcuerdo
        {
            get { return _isacuerdo; }
            set { _isacuerdo = value; OnPropertyChanged(); }
        }

        private bool _isterminos;

        public bool IsTerminos
        {
            get { return _isterminos; }
            set { _isterminos = value; OnPropertyChanged(); }
        }

        #endregion

        #region COMANDOS
        public Command RegistrarCommand { get; set; }

        public Command FotoCommand { get; set; }

        public Command AcuerdoCommand { get; set; }

        public Command TerminosCommand { get; set; }
        #endregion

        public RegistroViewModel(INavigation navigation, PersonaModel persona, bool verifcado)
        {
            _verifcado = verifcado;
            Navigation = navigation;
            _identificacion = persona.identificacion;
            _nombres = persona.nombres;
            _apellidos = persona.apellidos;
            _correo = persona.correo;
            _nacionalidad = "62883ca68d0ce7cb7d438059";
            _celular = persona.celular;
            fotoViewModel = new FotoViewModel();

            RegistrarCommand = new Command(() => Registrar());
            FotoCommand = new Command(() => takefoto());
            AcuerdoCommand = new Command(() => AcuerdoResp());
            TerminosCommand = new Command(() => Terminos());
        }

        private async void Registrar()
        {
            try
            {
                if (!_verifcado)
                {
                    await DisplayAlert("Alerta !", "No se verificó su cédula, debe actualizar sus datos caso contrario sus incidentes reportados pueden no se atendidos", "Cerrar");                   
                }
                if (await VerifyPassword() && await VerifyCheks() && await VerifyFoto())
                {
                    UserDialogs.Instance.ShowLoading("Registrando...");
                    PersonaModel persona = new PersonaModel
                    {
                        identificacion = _identificacion,
                        nombres = _nombres,
                        apellidos = _apellidos,
                        nacionalidad = _nacionalidad,
                        correo = _correo,
                        password = ConfirmPass,
                        fotoCedula = ConvertImgBase64.ConvertImgToBase64(fotoViewModel.PathFoto),
                        celular = _celular
                    };

                    Uri RequestUri = new Uri("http://incidentes-amt.herokuapp.com/personas/createPersona");
                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(persona);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(RequestUri, contentJson);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DisplayAlert("Mensaje", "Registrado correctamente", "Ok");
                        UserDialogs.Instance.HideLoading();
                        await Navigation.PushAsync(new Login(), true);
                    }
                    else
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        MessageModel obj = JsonConvert.DeserializeObject<MessageModel>(content);
                        var messageType = obj.message;

                        UserDialogs.Instance.HideLoading();
                        if(messageType != null)
                        {
                            await DisplayAlert("Ocurrió un error", messageType[0].ToString(), "Cerrar");
                        }
                        else
                        {
                            await DisplayAlert("Ocurrió un error", "No se pudo registrar", "Cerrar");
                        }
                    }
                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", "Ocurrión error al registrar, verifique que está enviando todos los datos", "Cerrar");
            }
        }

        private async Task<bool> VerifyFoto()
        {          
            if (FotoCedula == null || FotoCedula.IsEmpty)
            {
                await DisplayAlert("Error", "Debe agregar una foto de su cédula", "Cerrar");
                return false;
            }
            else
            {
                return true;
            }
        }

        private async Task<bool> VerifyCheks()
        {
            if(!IsAcuerdo || !IsTerminos)
            {
                await DisplayAlert("Error", "Debe aceptar los Terminos y Condiciónes y el Acuerdo de Responsabilidad,  ", "Cerrar");
                return false;
            }
            else
            {
                return true;
            }
        }

        private async Task<bool> VerifyPassword()
        {
           if(string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPass))
           {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Cerrar");
                return false;
           }
           else if(Password != ConfirmPass)
           {
                await DisplayAlert("Error", "Las contraseñas no son iguales", "Cerrar");
                return false;
           }
           else
           {
                return true;
           }
        }

        private async void takefoto()
        {
            fotoViewModel = new FotoViewModel();
            await fotoViewModel.TomarFoto();
            FotoCedula = fotoViewModel.PathFoto;
        }


        private async void AcuerdoResp()
        {
            await Navigation.PushAsync(new AcuerdoResposabilidad());
        }

        private async void Terminos()
        {
            await Navigation.PushAsync(new Terminos_y_condiciones());
        }
    }
}
