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


        public class MessageRegistroModel
        {
            public MessageRegistro message { get; set; }
        }

        public class MessageRegistro
        {
            public int code { get; set; }
            public string message { get; set; }
        }

        public class Return
        {
            public int code { get; set; }
            public string message { get; set; }
        }

        public class Root
        {
            public Return @return { get; set; }
        }


    }
}



