using Acr.UserDialogs;
using IncidentesAMT.Model;
using IncidentesAMT.View;
using IncidentesAMT.Vista;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IncidentesAMT.ViewModel
{
    public class PerfilUsuarioViewModel : BaseViewModel
    {
        #region VARIABLES
        string _idUser;
        private InfoUserByIdModel _infoUserModel;
        #endregion

        #region PROPIEDADES
        public INavigation Navigation { get; set; }

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

        #endregion

        #region COMMANDS

        public Command RefreshCommand { get; set; }
        public Command IncidentesCommand { get; set; }
        public Command DatosPeronalesCommand { get; set; }
        public Command AcuerdoCommand { get; set; }
        public Command CerrarSesionCommand { get; set; }
        #endregion

        public PerfilUsuarioViewModel(INavigation navigation, string idUser)
        {
            RefreshCommand = new Command(() => GetPersonaById());
            IncidentesCommand = new Command(() => InicidentesPage());
            DatosPeronalesCommand = new Command(() => DatosPersonalesPage());
            AcuerdoCommand = new Command(() => AcuerdoPage());
            CerrarSesionCommand = new Command(() => CerrarSesion());
            Navigation = navigation;
            _idUser = idUser;
            GetPersonaById();
        }

        private async void GetPersonaById()
        {
            try
            {
                IsBusy = true;
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://incidentes-amt.herokuapp.com/personas/" + _idUser);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accpet", "application/json");
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    _infoUserModel = JsonConvert.DeserializeObject<InfoUserByIdModel>(content);
                    Nombre = _infoUserModel.nombres;
                    Apellido = _infoUserModel.apellidos;
                    Telefono = (string)_infoUserModel.telefono;
                    Correo = _infoUserModel.correo;
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await DisplayAlert("Error", ex.Message.ToString(), "Cerrar");
            }

        }

        private async void InicidentesPage()
        {
            await Navigation.PushAsync(new IncidenteByUsuario(_idUser));
        }

        private async void DatosPersonalesPage()
        {
            await Navigation.PushAsync(new DatosPersonales(_idUser));
        }

        private async void AcuerdoPage()
        {
            await Navigation.PushAsync(new AcuerdoResposabilidad());
        }

        private async void CerrarSesion()
        {
            Preferences.Remove("UserId");
            Preferences.Clear();
            await Navigation.PushAsync(new Login(), true);
        }
    }
}
