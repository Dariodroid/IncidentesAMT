using IncidentesAMT.Helpers;
using IncidentesAMT.Modelo;
using IncidentesAMT.VistaModelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IncidentesAMT.ViewModel;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Plugin.Media.Abstractions;
using Acr.UserDialogs;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Essentials;

namespace IncidentesAMT.ViewModel
{
    public class IncidenteViewModel : BaseViewModel
    {
        Position position;
        #region VARIABLES
        public Xamarin.Forms.GoogleMaps.Map MapView;
        GeoLocation geoLocation = new GeoLocation();
        int cont = 0;
        bool geo;
        #endregion

        #region COMANDOS
        public Command ReportarIncidenteCommand { get; set; }
        public Command CapturarCommand { get; set; }
        #endregion

        #region PROPIEDADES
        private string _idPersona { get; set; }
        private string _idIncidente { get; set; }
        public INavigation Navigation { get; set; }
               
        private int _time;

        public int Time
        {
            get { return _time; }
            set { _time = value;OnPropertyChanged(); }
        }

        private string _direccion;

        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; OnPropertyChanged(); }
        }

        private string _descripcion;

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; OnPropertyChanged(); }
        }

        private ImageSource _foto;

        public ImageSource Foto
        {
            get { return _foto; }
            set { _foto = value; OnPropertyChanged(); }
        }

        private string _pathFoto;

        public string PathFoto
        {
            get { return _pathFoto; }
            set { _pathFoto = value; OnPropertyChanged(); }
        }

        private string _pathFoto2;

        public string PathFoto2
        {
            get { return _pathFoto2; }
            set { _pathFoto2 = value; OnPropertyChanged(); }
        }
        #endregion

        public IncidenteViewModel(INavigation navigation, string idPersona, string idIncidente, Xamarin.Forms.GoogleMaps.Map Map)
        {
            Navigation = navigation;
            _idPersona = idPersona;
            _idIncidente = idIncidente;
            MapView = Map;
            configMap();
            ReportarIncidenteCommand = new Command(async () => await Incidente());
            CapturarCommand = new Command(async () => await TomarFoto());
        }

        public async Task Incidente()
        {
            try
            {
                if (await VerifyIncidente())
                {
                    bool cancelReport = false;
                    Time = 10;
                    var message = $"Tiene {Time} segundos para cancelar su reporte";
                    using (var dialog = UserDialogs.Instance.Loading(message, () =>
                    cancelReport = true, "CANCELAR"))
                    {
                        Time--;
                        for (int i = 0; i <= 10; i++)
                        {
                            await Task.Delay(1000);
                            if (!cancelReport)
                            {
                                dialog.Title = $"Tiene {Time--} segundos para cancelar su reporte";
                                if (i == 10)
                                {
                                    await Reportar();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task Reportar()
        {
            try
            {               
                UserDialogs.Instance.ShowLoading("Reportando incidente espere...");
                IncidenteModel incidente = new IncidenteModel()
                {
                    direccion = Direccion,
                    latitud = GeoLocation.lat,
                    longitud = GeoLocation.lng,
                    persona = _idPersona,
                    fotoUno = ConvertImgBase64.ConvertImgToBase64(PathFoto),
                    fotoDos = ConvertImgBase64.ConvertImgToBase64(PathFoto2),
                    tipoIncidente = _idIncidente,
                    descripcion = Descripcion
                };

                Uri RequestUri = new Uri("https://incidentes-amt.herokuapp.com/incidentes/");
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(incidente);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Mensaje", "Incidente Registrado correctamente", "Ok");

                    var key = Preferences.Get("UserId", "");
                    if (key != "")
                    {
                        Application.Current.MainPage = new NavigationPage(new Menu(key));
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", response.StatusCode.ToString(), "Ok");
                }            
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message.ToString(), "Cerrar");
            }           
        }       

        private async Task<bool> VerifyIncidente()
        {
            if (string.IsNullOrEmpty(Direccion))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(PathFoto))
            {
                await DisplayAlert("Error", "Debe enviar una foto", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(PathFoto2))
            {
                await DisplayAlert("Error", "Debe enviar una foto", "Ok");
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task TomarFoto()
        {
            try
            {
                var camera = new StoreCameraMediaOptions();
                camera.PhotoSize = PhotoSize.Full;
                camera.SaveToAlbum = true;
                var foto = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(camera);
                if (foto != null)
                {
                    cont += 1;
                    if (cont == 1)
                    {
                        PathFoto = foto.Path;
                    }
                    else
                    {
                        PathFoto2 = foto.Path;
                        cont = 0;
                    }
                    Foto = ImageSource.FromStream(() =>
                    {
                        return foto.GetStream();
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error con la cámara  ${ ex.Message.ToString()}", "Cerrar");
            }
        }

        private void configMap()
        {
            MapView.UiSettings.CompassEnabled = true;
            MapView.UiSettings.MyLocationButtonEnabled = true;
            MapView.UiSettings.MapToolbarEnabled = true;
            MapView.MyLocationEnabled = true;
            MapView.FlowDirection = FlowDirection.LeftToRight;
            MapView.MapType = MapType.Street;
            moveToActualPosition();
        }

        public void moveToActualPosition()
        {
            Device.BeginInvokeOnMainThread( async () =>
            {
                position = new Position(GeoLocation.lat, GeoLocation.lng);
                MapView.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMeters(250)), true);
                adr();
            });
        }

        private async void adr()
        {
            var g = new Geocoder();
            var ad = await g.GetAddressesForPositionAsync(position);
            List<string> address = new List<string>();
            foreach (var item in ad)
            {
                address.Add(item);
            }
            var c1 = address[0].Split(',');
            var c2 = address[1].Split(',');
            var c3 = address[2].Split(',');
            var c4 = address[3].Split(',');
            var c5 = address[4].Split(',');
            Direccion = $"{c5[0].ToString()} - {c4[0].ToString()} - {c1[0].ToString()} - {c2[0].ToString()} - {c3[0].ToString()}";
        }

    }
}
