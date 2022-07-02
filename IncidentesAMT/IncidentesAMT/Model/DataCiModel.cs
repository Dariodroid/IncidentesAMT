using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentesAMT.Model
{
    public class DataCiModel
    {

        public class Root
        {
            public string codRetorno { get; set; }
            public Retorno retorno { get; set; }
        }

        public class Retorno
        {
            public string apellido1 { get; set; }
            public string apellido2 { get; set; }
            public string celular { get; set; }
            public string datos { get; set; }
            public string direccion { get; set; }
            public string domLocalidad { get; set; }
            public string domPais { get; set; }
            public string domProvincia { get; set; }
            public string email { get; set; }
            public string estadoCivil { get; set; }
            public string fechaNacimiento { get; set; }
            public string idPersona { get; set; }
            public string infracciones { get; set; }
            public Licencia[] licencias { get; set; }
            public string nacionalidad { get; set; }
            public string nombreCompleto { get; set; }
            public string nombres { get; set; }
            public string puntos { get; set; }
            public string restricciones { get; set; }
            public Resultado resultado { get; set; }
            public string sexo { get; set; }
            public string tipoPersona { get; set; }
            public string tipoSangre { get; set; }
        }

        public class Resultado
        {
            public string exito { get; set; }
        }

        public class Licencia
        {
            public string fechaDesde { get; set; }
            public string fechaHasta { get; set; }
            public string tipo { get; set; }
        }

    }

}
