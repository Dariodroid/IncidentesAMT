using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using IncidentesAMT.Modelo;
using System.Net.Http;
using System.Net;
using System.IO;
using IncidentesAMT.VistaModelo;
using IncidentesAMT.Helpers;
using IncidentesAMT.Model;
using System.Collections.ObjectModel;
using IncidentesAMT.ViewModel;

namespace IncidentesAMT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro : ContentPage
    {
        public Registro(PersonaModel persona)
        {
            InitializeComponent();
            BindingContext = new RegistroViewModel(Navigation, persona);
        }
    }
}