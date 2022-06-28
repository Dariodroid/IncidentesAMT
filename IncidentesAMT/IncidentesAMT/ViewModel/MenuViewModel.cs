using Acr.UserDialogs;
using IncidentesAMT.Helpers;
using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using IncidentesAMT.ViewModel;
using IncidentesAMT.Vista;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IncidentesAMT.VistaModelo
{
    public class MenuViewModel : BaseViewModel
    {
        #region COMMANDS
        public Command RefreshCommand { get; set; }
        public Command IncidenteSelectCommand { get; set; }
        public Command PerfilUserCommand { get; set; }

        #endregion

        #region VARIABLES
        string content;
        private ObservableCollection<IncidenteByPersonaModel> _incidenteByPersonaModel;
        string _lblIncActivos;
        string _lblfalso;
        private bool _noInternet;
        bool _frmActivos;
        bool _frmFalsos;
        string _idUser;
        int contgen = 0;
        int contfal = 0;
        #endregion

        #region OBJETOS
        public INavigation Navigation { get; set; }
        public bool FrmFalsos
        {
            get { return _frmFalsos; }
            set { _frmFalsos = value; OnPropertyChanged(); }
        }
        public bool FrmActivos
        {
            get { return _frmActivos; }
            set { _frmActivos = value; OnPropertyChanged(); }
        }
        public string Lblfalso
        {
            get { return _lblfalso; }
            set { _lblfalso = value; OnPropertyChanged(); }
        }
        public string LblIncActivos
        {
            get { return _lblIncActivos; }
            set { _lblIncActivos = value; OnPropertyChanged(); }
        }
        public bool NoInternet
        {
            get { return _noInternet; }
            set { _noInternet = value; OnPropertyChanged(); }
        }
        private bool _isConnected;

        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; OnPropertyChanged(); }
        }

        public ObservableCollection<IncidenteByPersonaModel> IncidenteByPersonaModel
        {
            get { return _incidenteByPersonaModel; }
            set { _incidenteByPersonaModel = value; OnPropertyChanged(); }
        }
        private ObservableCollection<CatalogoXIdModel> _incidentes;

        public ObservableCollection<CatalogoXIdModel> Incidentes
        {
            get { return _incidentes; }
            set { _incidentes = value; OnPropertyChanged(); }
        }

        #endregion

        public MenuViewModel(INavigation navigation, string idUser)
        {
            Navigation = navigation;
            _idUser = idUser;            
            VerifyInternet();
            RefreshCommand = new Command(() => GetIncidentePersonaById());
            IncidenteSelectCommand = new Command<CatalogoXIdModel>((C) => IncidenteSelect(C));
            PerfilUserCommand = new Command(() => PerfilUser());
        }

        private void VerifyInternet()
        {
            int cont = 0;
            var time = TimeSpan.FromSeconds(1);
            Device.StartTimer(time,  () =>
            {
                bool isconected = TestConnection();
                Device.BeginInvokeOnMainThread( () =>
                {
                    if (isconected)
                    {
                        NoInternet = false;
                        IsConnected = true;
                        if(IsConnected && cont == 0)
                        {
                            cont++;
                            GetIncidentePersonaById();
                            GetCatalogoXId();
                        }
                    }
                    else
                    {
                        NoInternet = true;
                        IsConnected = false;
                        cont = 0;
                    }
                });
                return true;              
            });
           
        }

        private bool TestConnection()
        {
            if (NetworkState.iHaveInternet())
            {             
                return true;
            }
            else
            {                
                return false;
            }
        }

        public async void GetIncidentePersonaById()
        {
            try
            {
                IsBusy = true;
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://incidentes-amt.herokuapp.com/incidentes/findByIdPersona/" + _idUser);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accpet", "application/json");
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    content = await response.Content.ReadAsStringAsync();
                    IncidenteByPersonaModel = JsonConvert.DeserializeObject<ObservableCollection<IncidenteByPersonaModel>>(content);
                    UserDialogs.Instance.HideLoading();
                    VerificarNotif();
                    IsBusy = false;
                }
               
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await DisplayAlert("Error", ex.Message.ToString(), "ok");
            }

        }

        private void VerificarNotif()
        {
            contgen = 0;
            contfal = 0;
            if (_incidenteByPersonaModel != null)
            {
                for (int i = 0; i < _incidenteByPersonaModel.Count; i++)
                {
                    if (_incidenteByPersonaModel[i].estado.ToString() == "GEN")
                    {
                        FrmActivos = true;
                        contgen ++;
                        LblIncActivos = $"{contgen} Incidente{(contgen > 1 ? "s" : "")} activo{(contgen > 1 ? "s" : "")}";
                    }

                    if (_incidenteByPersonaModel[i].estado.ToString() == "CAN")
                    {
                        FrmFalsos = true;
                        contfal ++;
                        Lblfalso = $"{contfal} Incidente{(contfal > 1 ? "s" : "")} falso{(contfal > 1 ? "s" : "")}";
                    }

                }
            }
        }

        private async void GetCatalogoXId()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://incidentes-amt.herokuapp.com/catalogo/findIdPadre/62ba9522a5702d2d5d1e813e");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accpet", "application/json");
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Incidentes = JsonConvert.DeserializeObject<ObservableCollection<CatalogoXIdModel>>(content);
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message.ToString(), "ok");
            }
        }

        private async void IncidenteSelect(CatalogoXIdModel catalogo)
        {
            if(contfal < 4)
            {
                var geo = await new GeoLocation().getLocationGPS();
                var city = await new GeoLocation().GetAddress();
                var limits = new GeoLocation().InPoligon();
                if (geo)
                {
                    // verifico si el area es diferente al DMQ
                    if (city.SubAdminArea != "Distrito Metropolitano de Quito")
                    {
                        await DisplayAlert("Alerta !", $"Usted está en {city.City}, no se puede reportar incidentes fuera de Quito", "Ok");
                        return;
                    }

                    // verifico si esta en el poligono y el DMQ
                    if (!limits && city.SubAdminArea == "Distrito Metropolitano de Quito")                    
                    {
                        await DisplayAlert("Alerta !", $"Usted está en el DQM, pero fuera de los limites designados", "Ok");
                        return;
                    }
                    await Navigation.PushAsync(new Incidente(_idUser, catalogo._id));
                }
            }
            else
            {
                await DisplayAlert("Alerta", "Usuario bloqueado por 180 días, superó el límite de incidentes falsos reportados","Ok");
                Application.Current.MainPage = new NavigationPage(new AcuerdoResposabilidad());
            }
        }

        private async void PerfilUser()
        {
            await Navigation.PushAsync(new PerfilUsuario(_idUser));
        }
    }
}
