using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IncidentesAMT.Helpers;
using Xamarin.Forms.GoogleMaps;
using IncidentesAMT.Modelo;
using IncidentesAMT.VistaModelo;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using IncidentesAMT.ViewModel;

namespace IncidentesAMT.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Incidente : ContentPage
    {
        GeoLocation geoLocation = new GeoLocation();
        string _idPersona;
        string _idIncidente;
        FotoViewModel fotoViewModel;
        //ConvertImgBase64 convertImg;
        public Incidente(string idPersona, string idIncidente)
        {
            _idIncidente = idIncidente; 
            _idPersona = idPersona;
            InitializeComponent();
            configMap();
            moveToActualPosition();
            fotoViewModel = new FotoViewModel();
            BindingContext = new IncidenteViewModel(Navigation,_idPersona,_idIncidente);
        }

        private void configMap()
        {
            MapView.UiSettings.CompassEnabled = true;
            MapView.UiSettings.MyLocationButtonEnabled = true;
            MapView.UiSettings.MapToolbarEnabled = true;
            MapView.MyLocationEnabled = true;
            MapView.FlowDirection = FlowDirection.LeftToRight;
            MapView.MapType = MapType.Street;
        }

        public void moveToActualPosition()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await geoLocation.getLocationGPS();
                Position position = new Position(GeoLocation.lat, GeoLocation.lng);
                MapView.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMeters(250)), true);
            });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                //convertImg = new ConvertImgBase64();
                IncidenteModel incidente = new IncidenteModel()
                {
                    direccion = txtDireccion.Text,
                    latitud = GeoLocation.lat,
                    longitud = GeoLocation.lng,
                    persona = _idPersona,
                    fotoUno = ConvertImgBase64.ConvertImgToBase64(fotoViewModel.PathFoto),
                    fotoDos = ConvertImgBase64.ConvertImgToBase64(fotoViewModel.PathFoto2),
                    tipoIncidente = _idIncidente,
                    descripcion = txtDescripcion.Text
                };

                Uri RequestUri = new Uri("http://incidentes-amt.herokuapp.com/incidentes/");
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(incidente);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    await DisplayAlert("Mensaje", "Incidente Registrado correctamente", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", response.StatusCode.ToString(), "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }

        }
    }
}