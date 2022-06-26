using IncidentesAMT.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IncidentesAMT.ViewModel
{
    public class GeoLocation : BaseViewModel
    {
        public static double lat { get; set; }

        public static double lng { get; set; }


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
                    lat = location.Latitude;    
                    lng = location.Longitude;
                    InPoligon();
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

        public bool InPoligon()
        {
            string[] poligon = {
                    "0.021867:  -78.498062",
                    "-0.004002:  -78.528904",
                    "-0.022052:  -78.477204",
                    "-0.049494:   -78.495418",
                    "-0.068423:   -78.495151",
                    "-0.072269:   -78.488571",
                    "-0.077411:   -78.496860",
                    "-0.076052:   -78.503039",
                    "-0.079910:   -78.504813",
                    "-0.077833:   -78.527076",
                    "-0.086549:   -78.534138",
                    "-0.098271:   -78.538210",
                    "-0.112385:   -78.535466",
                    "-0.103252:   -78.517592",
                    "-0.114625:   -78.511434",
                    "-0.125179:   -78.531197",
                    "-0.168452:   -78.501821",
                    "-0.177036:   -78.519313",
                    "-0.196053:   -78.520120",
                    "-0.204454:   -78.510256",
                    "-0.205502:   -78.515050",
                    "-0.200393:   -78.524790",
                    "-0.253077:   -78.555720",
                    "-0.251722:   -78.565555",
                    "-0.273532:   -78.586753",
                    "-0.339329:   -78.582543",
                    "-0.356277:   -78.545081",
                    "-0.335837:   -78.527332",
                    "-0.329631:   -78.521328",
                    "-0.311251:   -78.517737",
                    "-0.315934:   -78.505498",
                    "-0.337317:   -78.502195",
                    "-0.366427:   -78.476492",
                    "-0.308421:   -78.445590",
                    "-0.323721:   -78.412276",
                    "-0.395314:   -78.377958",
                    "-0.394251:   -78.355715",
                    "-0.299326:   -78.380903",
                    "-0.276968:   -78.441677",
                    "-0.221079:   -78.457838",
                    "-0.232989:   -78.452178",
                    "-0.237597:   -78.442553",
                    "-0.237732:   -78.388146",
                    "-0.223983:   -78.367036",
                    "-0.196423:   -78.376736",
                    "-0.183168:   -78.412509",
                    "-0.137784:   -78.409692",
                    "-0.228110:   -78.352265",
                    "-0.241182:   -78.336708",
                    "-0.234154:   -78.325704",
                    "-0.093299:   -78.274166",
                    "-0.069019:   -78.286150",
                    "-0.045729:   -78.335648",
                    "-0.044439:   -78.430316",
                    "-0.001763:   -78.427046",
                    "0.016234:    -78.451513",
                    "0.023968:    -78.498461",
            };
            string latPolig = string.Empty;
            string lngPolig = string.Empty;
            bool isPoligon = false;

            for (int i = 0; i < poligon.Length; i++)
            {
                latPolig = poligon[i].Split(':')[0];
                lngPolig = poligon[i].Split(':')[1];

                if (lat < Convert.ToDouble(latPolig) && lng < Convert.ToDouble(lngPolig))
                {
                    isPoligon = false;
                }
                else
                {
                    isPoligon = true;
                }
            }
            return isPoligon;
        }
    }
}
