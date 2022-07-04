using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentesAMT.Model
{
    public class SendMailModel
    {
        public string to { get; set; }
        public string subject { get; set; }
        public string html { get; set; }


        public class MessageEmailSendModel
        {
            public string message { get; set; }
            public string message_id { get; set; }
        }


    }
}
