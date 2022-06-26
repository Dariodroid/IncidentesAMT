using IncidentesAMT.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IncidentesAMT.Interfaces
{
    public interface IReverseGeocode
    {
        Task<LocationAddress> GetLocationAddress(double latitude, double longitude);
    }
}
