using Acr.UserDialogs;
using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IncidentesAMT.ViewModel
{
    public class CambiarPasswordViewModel : BaseViewModel
    {
        #region VARIABLES
        private string _idUser;
        #endregion

        #region PROPIEDADES

        public INavigation Navigation { get; set; }


        private string _oldPassword;

        public string OldPassword
        {
            get { return _oldPassword; }
            set { _oldPassword = value; OnPropertyChanged(); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _newPassword;

        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; OnPropertyChanged(); }
        }

        private ImageSource _fotoPerfil;

        public ImageSource FotoPerfil
        {
            get { return _fotoPerfil; }
            set
            {
                _fotoPerfil = value;
                OnPropertyChanged();
            }
        }

        private string _telefono;

        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; OnPropertyChanged(); }
        }

        private string _correo;

        public string Correo
        {
            get { return _correo; }
            set { _correo = value; OnPropertyChanged(); }
        }

        private string _nombre;

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged(); }
        }
        
        private string _apellido;

        public string Apellido 
        {
            get { return _apellido; }
            set { _apellido = value; OnPropertyChanged(); }
        }

        private string _iconOldPass;

        public string IconNameOldpass
        {
            get { return _iconOldPass; }
            set { _iconOldPass = value; OnPropertyChanged(); }
        }

        private string _iconPass;

        public string IconPass
        {
            get { return _iconPass; }
            set { _iconPass = value; OnPropertyChanged(); }
        }

        private string _iconNewPass;

        public string IconNewPass
        {
            get { return _iconNewPass; }
            set { _iconNewPass = value; OnPropertyChanged(); }
        }

        private bool _showOldPassword;

        public bool ShowOldPassword
        {
            get { return _showOldPassword; }
            set { _showOldPassword = value; OnPropertyChanged(); }
        }

        private bool _showPassword;

        public bool ShowPassword
        {
            get { return _showPassword; }
            set { _showPassword = value; OnPropertyChanged(); }
        }

        private bool _showNewPassword;

        public bool ShowNewPassword
        {
            get { return _showNewPassword; }
            set { _showNewPassword = value; OnPropertyChanged(); }
        }

        #endregion

        #region COMANDOS
        public Command UpdatePasswordCommand { get; set; }
        public Command IsPasswordOldCommand { get; set; }
        public Command IsPasswordCommand { get; set; }
        public Command IsNewPasswordCommand { get; set; }
        #endregion

        public CambiarPasswordViewModel(INavigation navigation, InfoUserByIdModel cambiarPasswordModel, ImageSource fotoPerfil)
        {
            Navigation = navigation;
            _idUser = cambiarPasswordModel._id;
            FotoPerfil = fotoPerfil;
            Telefono = cambiarPasswordModel.telefono;
            Correo = cambiarPasswordModel.correo;
            Nombre = cambiarPasswordModel.nombres;
            Apellido = cambiarPasswordModel.apellidos;
            Pass();
            UpdatePasswordCommand = new Command(() => UpdatePassword());
            IsPasswordOldCommand = new Command(() => IsPasswordOld());
            IsPasswordCommand = new Command(() => IsPassword());
            IsNewPasswordCommand = new Command(() => IsNewPassword());
        }

        private void IsPassword()
        {
            if (!ShowPassword)
            {
                ShowPassword = true;
                IconPass = "eye";
            }
            else if (ShowPassword)
            {
                ShowPassword = false;
                IconPass = "hide";
            }
        }

        private void IsPasswordOld()
        {
            if (!ShowOldPassword)
            {
                ShowOldPassword = true;
                IconNameOldpass = "eye.png";
            }
            else if (ShowOldPassword)
            {
                ShowOldPassword = false;
                IconNameOldpass = "hide";
            }
        }

        private void IsNewPassword()
        {
            if (!ShowNewPassword)
            {
                ShowNewPassword = true;
                IconNewPass = "eye";
            }
            else if (ShowNewPassword)
            {
                ShowNewPassword = false;
                IconNewPass = "hide";
            }
        }

        private async void UpdatePassword()
        {
            try
            {
                if(await VerifyPassword()) { 
                    
                    UserDialogs.Instance.ShowLoading("Actualizando...");

                    CambiarPasswordModel passwordModel = new CambiarPasswordModel
                    {
                        passwordOld = OldPassword,
                        password = NewPassword                        
                    };
                    Uri RequestUri = new Uri("http://servicios.amt.gob.ec:5001/personas/updatePassword/" + _idUser);
                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(passwordModel);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(RequestUri, contentJson);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Mensaje", "Contraseña actualizada correctamete", "Ok");
                        Preferences.Remove("UserId");
                        Preferences.Clear();
                        Application.Current.MainPage = new NavigationPage(new Login());
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Error", "Ocurrió un error al actualizar, revise su contraseña actual", "Ok");
                    }
                }
            }
            
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task<bool> VerifyPassword()
        {
            if (Password != NewPassword)
            {
                await DisplayAlert("Error", "Las contraseñas no son iguales", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(OldPassword))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Cerrar");
                return false;
            }
            if(NewPassword == OldPassword)
            {
                await DisplayAlert("Error", "La nueva contraseña no debe ser igual a la actual", "Cerrar");
                return false;
            }

            return true;
        }

        private void Pass()
        {
            ShowOldPassword = true;
            IconNameOldpass = "eye.png";

            ShowPassword = true;
            IconPass = "eye";

            ShowNewPassword = true;
            IconNewPass = "eye";
        }
    }
}
