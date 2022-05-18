using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace IncidentesAMT.Modelo
{
    public class FotoModel
    {
        private ImageSource _foto;

        public ImageSource Foto
        {
            get { return _foto; }
            set { _foto = value;}
        }

        private string _pathFoto;

        public string PathFoto
        {
            get { return _pathFoto; }
            set { _pathFoto = value; }
        }


    }
}
