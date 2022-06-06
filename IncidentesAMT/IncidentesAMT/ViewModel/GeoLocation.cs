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
                await DisplayAlert("Error", "Los servicios de localización no están activos en éste dispositivo, por favor active el GPS", "Cerrar");
                NavegarToMain();
                return false;
            }
            catch (PermissionException ex)
            {
                // Handle permission exception               
                await DisplayAlert("Permisos de Ubicación", "No se concedió permiso a la aplicación para usar su ubicación, para poder reportar un incidente debe permitir el acceso a su ubicación en las configuraiónes del dispositivo", "Cerrar");
                NavegarToMain();
                return false;
            }
            catch (Exception ex)
            {
                // Unable to get location
                await DisplayAlert("Error", "No se puede obtener la ubicación", "Cerrar");
                NavegarToMain();
                return false;
            }
        }

        private void NavegarToMain()
        {
            var key = Preferences.Get("UserId", "");
            if (key != "")
            {
                Application.Current.MainPage = new NavigationPage(new Menu(key));
            }
        }
    }
}
