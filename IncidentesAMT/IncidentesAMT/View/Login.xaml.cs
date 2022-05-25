using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using IncidentesAMT.Modelo;

namespace IncidentesAMT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private string _userId;
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                LoginModel loginModel = new LoginModel()
                {
                    correo = txtUser.Text.Trim(),
                    password = txtPassword.Text.Trim(),
                };

                Uri RequestUri = new Uri("http://incidentes-amt.herokuapp.com/auth/login");
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(loginModel);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    InfoUserModel infoUserObject = JsonConvert.DeserializeObject<InfoUserModel>(content);
                    _userId = infoUserObject._id;
                    Preferences.Set("Remember", RememberStw.IsToggled);
                    Preferences.Set("UserId", _userId);
                    txtUser.Text = String.Empty;
                    txtPassword.Text = String.Empty;
                    RememberStw.IsToggled=false;
                    await Navigation.PushAsync(new Menu(_userId), true);
                }
                else
                {
                    await DisplayAlert("Error", "Usuario o contraseña incorrectos", "Ok");
                }

            }
            catch (Exception ex)
            {
                if(string.IsNullOrEmpty(txtUser.Text) && string.IsNullOrEmpty(txtPassword.Text ))
                {
                    await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                    txtUser.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    await DisplayAlert("Error", "Debe ingresar su contraseña", "Ok");
                    txtPassword.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtUser.Text))
                {
                    await DisplayAlert("Error", "Debe ingresar un usuario", "Ok");                    
                    txtUser.Focus();
                    return;
                }
                else
                {
                    await DisplayAlert("Error", ex.Message.ToString(), "Ok");
                }
            }
        }

        protected override void OnAppearing()
        {
            if ((Preferences.Get("Remember", true) == true))
            {
                var key = Preferences.Get("UserId","");
                if(key != "")
                {
                    RememberStw.IsToggled = true;
                    Navigation.PushAsync(new Menu(key), true);
                }
            }
        }

        private async void btnCrearCunenta_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DatosPersona());
        }
    }
}