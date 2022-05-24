using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentesAMT.Model
{
    public class InfoUserByIdModel
    {
        public string _id { get; set; }
        public string identificacion { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public Nacionalidad nacionalidad { get; set; }
        public object direccion { get; set; }
        public string correo { get; set; }
        public object telefono { get; set; }
        public string password { get; set; }
        public int strikes { get; set; }
        public string bloqueo { get; set; }
        public string fotoPerfil { get; set; }
        public string fotoCedula { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaEdicion { get; set; }
        public string estado { get; set; }
        public int __v { get; set; }
    }

    public class Nacionalidad
    {
        public string _id { get; set; }
        public string idPadre { get; set; }
        public string nombre { get; set; }
        public string valor { get; set; }
        public DateTime fechaCreacion { get; set; }
        public object fechaEdicion { get; set; }
        public string estado { get; set; }
        public int __v { get; set; }
    }
}
