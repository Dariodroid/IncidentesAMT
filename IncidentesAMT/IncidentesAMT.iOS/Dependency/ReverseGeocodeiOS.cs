using Contacts;
using CoreLocation;
using Foundation;
using IncidentesAMT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IncidentesAMT.iOS.Dependency.ReverseGeocodeiOS))]
namespace IncidentesAMT.iOS.Dependency
{
    public class ReverseGeocodeiOS
    {
        public async Task<LocationAddress> GetLocationAddress(double latitude, double longitude)
        {
            var geoCoder = new CLGeocoder();
            var place = new CLLocation(latitude, longitude);
            var placemarks = await geoCoder.ReverseGeocodeLocationAsync(place);

            if (placemarks.Any())
            {
                var placeMark = placemarks.First();

                var location = new LocationAddress()
                {
                    Name = placeMark.Name,
                    City = placeMark.Locality,
                    Province = placeMark.AdministrativeArea,
                    ZipCode = placeMark.PostalCode,
                    Country = $"{placeMark.Country} ({placeMark.IsoCountryCode})",
                    SubAdminArea = placeMark.SubAdministrativeArea,
                    SubLocality = placeMark.SubLocality,
                    Address = new CNPostalAddressFormatter().StringFor(placeMark.PostalAddress)
                };

                return location;
            }

            return null;
        }
    }
}