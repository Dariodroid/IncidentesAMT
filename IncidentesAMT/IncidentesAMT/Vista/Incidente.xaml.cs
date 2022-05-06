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

namespace IncidentesAMT.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Incidente : ContentPage
    {
        //private string _direccion;
        //private double _latitud;
        //private double _longitud;
        //private string _persona;
        //private string _fotoUno;
        //private string _fotoDos;


        GeoLocation geoLocation = new GeoLocation();
        public Incidente()
        {
            InitializeComponent();
            configMap();
            moveToActualPosition();
            BindingContext = new FotoViewModel();
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            IncidenteModel incidente = new IncidenteModel()
            {
                direccion = txtDireccion.Text,
                latitud = GeoLocation.lat,
                longitud = GeoLocation.lng,
                persona = "idpersona",
                fotoUno = "foto1",
                fotoDos = "foto2"

            };
        }
    }
}