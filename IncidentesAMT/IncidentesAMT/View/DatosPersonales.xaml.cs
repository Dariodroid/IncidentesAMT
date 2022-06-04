using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using IncidentesAMT.ViewModel;
using IncidentesAMT.VistaModelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncidentesAMT.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatosPersonales : ContentPage
    {
        string _idUser;
        public DatosPersonales(string idUser)
        {
            InitializeComponent();
            _idUser = idUser;
            BindingContext = new DatosPersonalesViewModel(Navigation, _idUser);
        }
    }
}