using IncidentesAMT.Interfaces;
using IncidentesAMT.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IncidentesAMT.Helpers
{
    public static class LocationService
    {
        public static async Task<LocationAddress> GetAddress(double latitude, double longitude)
        {
            var service = DependencyService.Get<IReverseGeocode>();
            var location = await service.GetLocationAddress(latitude, longitude);
            return location;
        }
    }
}
