using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentesAMT.Model
{
    public class CatalogoModel
    {

        public class Catalogo
        {
            public ClassCatalogo[] Property1 { get; set; }
        }

        public class ClassCatalogo
        {
            public string _id { get; set; }
            public string idPadre { get; set; }
            public string nombre { get; set; }
            public string valor { get; set; }
            public string tipo { get; set; }
            public DateTime fechaCreacion { get; set; }
            public DateTime? fechaEdicion { get; set; }
            public string estado { get; set; }
            public int __v { get; set; }
        }

    }
}
