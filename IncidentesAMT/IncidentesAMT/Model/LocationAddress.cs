using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentesAMT.Model
{
    public class LocationAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string SubAdminArea { get; set; }
        public string SubLocality { get; set; }
    }
}
