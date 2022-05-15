using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentesAMT.Modelo
{
    public class InfoUserModel
    {
        public string _id { get; set; }
        public string identificacion { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string nacionalidad { get; set; }
        public object direccion { get; set; }
        public string correo { get; set; }
        public object telefono { get; set; }
        public string password { get; set; }
        public int strikes { get; set; }
        public string bloqueo { get; set; }
        public object fotoPerfil { get; set; }
        public string fotoCedula { get; set; }
        public DateTime fechaCreacion { get; set; }
        public object fechaEdicion { get; set; }
        public string estado { get; set; }
        public int __v { get; set; }
    }
}
