using IncidentesAMT.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace IncidentesAMT.Modelo
{
    public class IncidenteByPersonaModel
    {
        public string _id { get; set; }
        public string direccion { get; set; }
        public string descripcion { get; set; }
        public float latitud { get; set; }
        public float longitud { get; set; }
        public string tipoIncidente { get; set; }
        public string fotoUno { get; set; }
        public string fotoDos { get; set; }
        public string persona { get; set; }
        public object idAgente { get; set; }
        public DateTime fechaCreacion { get; set; }
        public object fechaEdicion { get; set; }
        public string estado { get; set; }
        public int __v { get; set; }
        public ImageSource MyImage
        {
            get { return ConvertImgBase64.GetImageSourceFromBase64String(fotoUno); }
        }
        public ImageSource MyImage2
        {
            get { return ConvertImgBase64.GetImageSourceFromBase64String(fotoDos); }
        }        

        public string Estado
        {
            get { 
                if(estado== "GEN")
                    return estado = "GENERADO";
                else
                    return estado = "ACTIVO";
            }
        }
    }
}
