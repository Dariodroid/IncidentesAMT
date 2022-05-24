using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentesAMT.Model
{
    public class MessageModel
    {
        public int statusCode { get; set; }
        public string[] message { get; set; }
        public string error { get; set; }
    }
}
