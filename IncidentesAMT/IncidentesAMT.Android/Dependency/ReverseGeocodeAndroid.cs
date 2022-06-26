using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IncidentesAMT.Interfaces;
using IncidentesAMT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(IncidentesAMT.Droid.Dependency.ReverseGeocodeAndroid))]
namespace IncidentesAMT.Droid.Dependency
{
    public class ReverseGeocodeAndroid : IReverseGeocode
    {
        public async Task<LocationAddress> GetLocationAddress(double latitude, double longitude)
        {
            var geoCoder = new Geocoder(Android.App.Application.Context);

            var g = Geocoder.IsPresent;

            var addresses = await geoCoder.GetFromLocationAsync(latitude, longitude, 1);

            if (addresses.Any())
            {
                var address = addresses.First();

                var location = new LocationAddress()
                {
                    Name = address.FeatureName,
                    City = address.Locality,
                    Province = address.AdminArea,
                    ZipCode = address.PostalCode,
                    Country = $"{address.CountryName} ({address.CountryCode})",      
                    SubAdminArea = address.SubAdminArea,
                    SubLocality = address.SubLocality,
                    Address = address.GetAddressLine(0)
                }; 

                return location;
            }

            return null;
        }
    }
}