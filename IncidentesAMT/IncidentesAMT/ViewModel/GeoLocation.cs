using Acr.UserDialogs;
using IncidentesAMT.Helpers;
using IncidentesAMT.Model;
using IncidentesAMT.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using static IncidentesAMT.Model.CatalogoModel;

namespace IncidentesAMT.ViewModel
{
    public class GeoLocation : BaseViewModel
    {
        #region PROPIEDADES
        public static double lat { get; set; }

        public static double lng { get; set; }

        private LocationAddress _locationAddress;

        ObservableCollection<ClassCatalogo> _catalogo;

        public ObservableCollection<ClassCatalogo> Catalogo
        {
            get { return _catalogo; }
            set { _catalogo = value; OnPropertyChanged(); }
        }
        public LocationAddress LocationAddress
        {
            get { return _locationAddress; }
            set { _locationAddress = value; OnPropertyChanged(); }
        }

        #endregion
        
        public static bool inPoligon { get; set; }
        public async Task<bool> getLocationGPS()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest()
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(30)
                });
                if (location != null)
                {
                    lat = /*-0.1328905;*/  location.Latitude;
                    lng = /*-78.4941563;*/ location.Longitude;
                    await GetAddress();
                    return true;  
                }
                
                else
                {
                    return false;
                    //var knowLocation = await Geolocation.GetLastKnownLocationAsync();
                    //lat = knowLocation.Latitude;
                    //lng = knowLocation.Longitude;
                }
                
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException ex)
            {
                await DisplayAlert("Error de Ubicación", "Los servicios de localización no están activos, por favor active el GPS", "Cerrar");
                return false;
            }
            catch (PermissionException ex)
            {
                // Handle permission exception               
                await DisplayAlert("Permisos de Ubicación", "No se concedió permiso a la aplicación para usar su ubicación, para poder reportar un incidente debe permitir el acceso a su ubicación en las configuraiónes del dispositivo", "Cerrar");
                return false;
            }
            catch (Exception ex)
            {
                // Unable to get location
                await DisplayAlert("Error", "No se puede obtener la ubicación", "Cerrar");
                return false;
            }
        }
      
        public async Task<LocationAddress> GetAddress()
        {
            LocationAddress = await LocationService.GetAddress(lat, lng);
            return LocationAddress;
        }

        public async Task<bool> InPoligon()
        {
            var res = await Getcatalogo();
            var latLngs = res;

            List<string> coord = new List<string>();
            List<string> vertices_y = new List<string>();
            List<string> vertices_x = new List<string>();          

            var r = 0;
            var i = 0;
            var j = 0;
            bool c = false;
            var point = 0;
            for (int d = 0; d < res.Length; d++)
            {
                if(res[d] != "")
                    coord.Add(res[d]);
            }

            for (r = 0; r < coord.Count-1; r++)
            {                
                vertices_y.Add(coord[r].Split(',')[0].Replace(',', '.'));
                vertices_x.Add(coord[r].Split(',')[1].Replace(',', '.'));
            }
            var points_polygon = vertices_x.Count;
            for (i = 0, j = points_polygon - 1; i < points_polygon; j = i++)
            {                
                point = i;
                if (point == points_polygon) point = 0;

                var op1 = (Convert.ToDouble(vertices_x[j]) - Convert.ToDouble(vertices_x[point]));
                var op2 = lat - Convert.ToDouble(vertices_y[point]);
                var op3 = Convert.ToDouble(vertices_y[j]) - Convert.ToDouble(vertices_y[point]);

                var total = (op1) * (op2) / (op3)+Convert.ToDouble(vertices_x[point]);

                if ((Convert.ToDouble(vertices_y[point]) > lat) != (Convert.ToDouble(vertices_y[j]) > lat) && lng < total)
                {
                    c = !c;
                }
            }
            inPoligon = c;
            return c;
        }

        public async Task<string[]> Getcatalogo()
        {
            string[] words = {};
            try
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://servicios.amt.gob.ec:5001/catalogo/");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accpet", "application/json");
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Catalogo = JsonConvert.DeserializeObject<ObservableCollection<ClassCatalogo>>(content);

                    string result;
                    for (int i = 0; i < Catalogo.Count - 1; i++)
                    {
                        var tipo = Catalogo[i].tipo;
                        string r = string.Empty;
                        if (tipo == "Mapa" && Catalogo[i].nombre == "Mapa Restricción Quito")
                        {
                            result = Regex.Replace(Catalogo[i].valor, "[\"[lat\":\"lng}\\]]", string.Empty);
                            words = result.Split('{');
                        }
                    }
                }
                return words;                   
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message.ToString(), "ok");
                return words;
            }
        }
    }
}
