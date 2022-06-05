using IncidentesAMT.Helpers;
using IncidentesAMT.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace IncidentesAMT.ViewModel
{
    public class Detalle_incidenteViewModel : BaseViewModel
    {
        #region VARIABLES
        public Xamarin.Forms.GoogleMaps.Map _map;
        public IncidenteByPersonaModel Incidente;

        private string _direccion;
        #endregion

        #region PROPIEDADES
        public INavigation Navigation { get; set; }

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

        #endregion

        public Detalle_incidenteViewModel( INavigation navigation, IncidenteByPersonaModel incidenteByPersonaModel, Xamarin.Forms.GoogleMaps.Map Map )
        {
            Navigation = navigation;
            _map = Map;
            Incidente = incidenteByPersonaModel;
            LoadData();
        }

        private void LoadData()
        {
            Direccion = Incidente.direccion;
            Descripcion = Incidente.descripcion;
            moveToActualPosition();
        }

        public void moveToActualPosition()
        {
            Position position = new Position(Incidente.latitud, Incidente.longitud);
            _map.UiSettings.CompassEnabled = false;
            _map.UiSettings.MyLocationButtonEnabled = false;
            _map.UiSettings.MapToolbarEnabled = true;
            _map.UiSettings.ZoomControlsEnabled = false;
            _map.FlowDirection = FlowDirection.LeftToRight;
            _map.Pins.Add(new Pin
            {
                Label= "Mi incidente",
                Position = position,
                
            });
            _map.FlowDirection = FlowDirection.LeftToRight;
            _map.MapType = MapType.Street;
            _map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMeters(250)), true);
        }
    }
}
