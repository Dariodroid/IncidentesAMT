using Acr.UserDialogs;
using IncidentesAMT.Helpers;
using IncidentesAMT.Modelo;
using IncidentesAMT.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IncidentesAMT.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userId;

        #region COMANDOS
        public Command LoginCommand { get; set; }
        public Command CrearteAccount { get; set; }
        public Command IsPasswordCommand { get; set; }
        public Command ResetPasswordCommand { get; set; }

        #endregion

        #region PROPIEDADES
        public INavigation Navigation { get; set; }

        private bool _isRemember = false;

        public bool IsRemember
        {
            get { return _isRemember; }
            set { _isRemember = value; OnPropertyChanged(); }
        }

        private string _user;

        public string User
        {
            get { return _user; }
            set { _user = value;OnPropertyChanged(); }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private bool _showPassword;

        public bool ShowPassword
        {
            get { return _showPassword; }
            set { _showPassword = value; OnPropertyChanged(); }
        }

        private string _iconPass;

        public string IconPass
        {
            get { return _iconPass; }
            set { _iconPass = value; OnPropertyChanged(); }
        }
        private string _Password { get; set; }
        #endregion

        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;
            LoginCommand = new Command(async () => await Login());
            CrearteAccount = new Command(async () => await CreateAccount());
            IsPassword();
            IsPasswordCommand = new Command(() => IsPassword());
            ResetPasswordCommand = new Command(async() => await ResetPassword());
        }

        public async Task Login()
        {
            try
            {
                if(await VerifyCred())
                {
                    if (!Verify_Email()) { await DisplayAlert("Error", "Ingrese un correo electrónico válido", "Ok"); return; }
                    UserDialogs.Instance.ShowLoading("Autenticando...");
                    LoginModel loginModel = new LoginModel()
                    {
                        correo = User,
                        password = Password,
                    };

                    Uri RequestUri = new Uri("http://servicios.amt.gob.ec:5001/auth/login");
                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(loginModel);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(RequestUri, contentJson);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        InfoUserModel infoUserObject = JsonConvert.DeserializeObject<InfoUserModel>(content);
                        _userId = infoUserObject._id;
                        Preferences.Set("Remember", IsRemember);
                        Preferences.Set("UserId", _userId);
                        UserDialogs.Instance.HideLoading();
                        Application.Current.MainPage = new NavigationPage(new Menu(_userId));
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Error", "Usuario o contraseña incorrectos", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message.ToString(), "Ok");
            }
        }

        private async Task<bool> VerifyCred()
        {
            UserDialogs.Instance.HideLoading();
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(User))
            {
                await DisplayAlert("Error", "Ingrese todos los campos", "Ok");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool Verify_Email()
        {
            return Verify_email.Verify(User);
        }

        public void Navegar()
        {
            if ((Preferences.Get("Remember", true) == true))
            {
                var key = Preferences.Get("UserId", "");
                if (key != "")
                {
                    IsRemember = true;
                    Application.Current.MainPage = new NavigationPage(new Menu(key));
                }
            }
        }

        public async Task CreateAccount()
        {
            await Navigation.PushAsync(new DatosPersona());
        }

        public async Task ResetPassword()
        {
            await Navigation.PushAsync(new RecuperarPassword());
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
    }

}
