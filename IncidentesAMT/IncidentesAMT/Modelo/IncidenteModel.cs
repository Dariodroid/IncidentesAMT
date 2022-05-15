using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentesAMT.Modelo
{
    public class IncidenteModel
    {
        public string direccion { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public string persona { get; set; }
        public string fotoUno { get; set; }
        public string fotoDos { get; set; }
        public string tipoIncidente { get; set; }
    }
}
